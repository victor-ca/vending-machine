import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_ENDPOINT } from './endpoint';
import { map, Observable, of, tap } from 'rxjs';
type AuthenticateResult = {
  token: string;
  refreshToken: string;
  expiration: string;
};

const tokenKey = 'vendingMachineToken';
@Injectable({
  providedIn: 'root',
})
export class AuthService {
  getAuthToken(): Observable<string | undefined> {
    try {
      const { token, expiration } = JSON.parse(
        localStorage.getItem(tokenKey)!
      ) as AuthenticateResult;

      return of(token);
    } catch (e) {
      console.error(e);
      return of(undefined);
    }
  }

  constructor(
    @Inject(API_ENDPOINT) private readonly apiEndpoint: string,
    private readonly httpClient: HttpClient
  ) {}

  register(userName: string, password: string): Observable<unknown> {
    return this.httpClient
      .post<AuthenticateResult>(`${this.apiEndpoint}/auth/register`, {
        userName,
        password,
      })
      .pipe(
        tap((result) => {
          localStorage.setItem(tokenKey, JSON.stringify(result));
        })
      );
  }

  login(userName: string, password: string): Observable<unknown> {
    return this.httpClient
      .post<AuthenticateResult>(`${this.apiEndpoint}/auth/login`, {
        userName,
        password,
      })
      .pipe(
        tap((result) => {
          localStorage.setItem(tokenKey, JSON.stringify(result));
        })
      );
  }
}
