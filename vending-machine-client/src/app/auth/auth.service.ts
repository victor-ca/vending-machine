import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_ENDPOINT } from './endpoint';
import {
  catchError,
  map,
  Observable,
  of,
  ReplaySubject,
  take,
  tap,
} from 'rxjs';

type AuthenticateResult = {
  token: string;
  refreshToken: string;
  expiration: string;
};

export type CurrentUser =
  | {
      isAuthenticated: false;
    }
  | { isAuthenticated: true; name: string; isSeller: boolean };

const tokenKey = 'vendingMachineToken';
import jwtDecode from 'jwt-decode';
@Injectable({
  providedIn: 'root',
})
export class AuthService {
  currentUser$ = new ReplaySubject<CurrentUser>(1);
  getAuthToken(): Observable<string | undefined> {
    try {
      const authState = JSON.parse(
        localStorage.getItem(tokenKey)!
      ) as AuthenticateResult;
      const { token, refreshToken, expiration } = authState;

      console.warn(
        `refresh in ${
          (new Date(expiration).getTime() - new Date().getTime()) / 1000
        }s`
      );

      if (new Date(expiration) < new Date()) {
        console.warn(`refresh`);

        return this.refresh({
          accessToken: token,
          refreshToken,
        }).pipe(
          map(({ token }) => token),
          catchError(() => {
            this.logOut();

            return of(undefined);
          })
        );
      }
      this.currentUser$.next(mapToCurrentUser({ token }));

      return of(token);
    } catch {
      this.logOut();
      return of(undefined);
    }
  }
  logOut(): Observable<CurrentUser> {
    localStorage.removeItem(tokenKey);
    this.currentUser$.next({ isAuthenticated: false });
    return of({ isAuthenticated: false });
  }

  constructor(
    @Inject(API_ENDPOINT) private readonly apiEndpoint: string,
    private readonly httpClient: HttpClient
  ) {}

  register(
    userName: string,
    password: string,
    isSeller: boolean
  ): Observable<CurrentUser> {
    return this.httpClient
      .post<AuthenticateResult>(`${this.apiEndpoint}/auth/register`, {
        userName,
        password,
        isSeller,
      })
      .pipe(tap(this.persistTokenAndUpdateUser), map(mapToCurrentUser));
  }

  login(userName: string, password: string): Observable<CurrentUser> {
    return this.httpClient
      .post<AuthenticateResult>(`${this.apiEndpoint}/auth/login`, {
        userName,
        password,
      })
      .pipe(tap(this.persistTokenAndUpdateUser), map(mapToCurrentUser));
  }

  private refresh(refreshRequest: {
    accessToken: string;
    refreshToken: string;
  }): Observable<AuthenticateResult> {
    return this.httpClient
      .post<AuthenticateResult>(
        `${this.apiEndpoint}/auth/refresh-token`,
        refreshRequest
      )
      .pipe(tap(this.persistTokenAndUpdateUser));
  }

  init() {
    return this.getAuthToken().pipe(
      take(1),
      map((token) => mapToCurrentUser({ token }))
    );
  }

  persistTokenAndUpdateUser = (authResult: AuthenticateResult): void => {
    localStorage.setItem(tokenKey, JSON.stringify(authResult));
    this.currentUser$.next(mapToCurrentUser(authResult));
  };
}
const mapToCurrentUser = ({
  token,
}: {
  token: string | undefined;
}): CurrentUser => {
  if (!token) {
    return { isAuthenticated: false };
  }

  var { isSeller, userName } = jwtDecode(token) as Record<string, string>;

  return {
    isAuthenticated: true,
    isSeller: isSeller === 'true',
    name: userName,
  };
};
