import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_ENDPOINT } from './endpoint';
import { map, Observable, of, tap } from 'rxjs';
type AuthenticateResult = {
  token: string;
};

const tokenKey = 'vendingMachineToken';
@Injectable({
  providedIn: 'root',
})
export class AuthService {
  getAuthToken(): Observable<string> {
    return of(localStorage.getItem(tokenKey)!);
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
        tap(({ token }) => {
          localStorage.setItem(tokenKey, token);
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
        tap(({ token }) => {
          localStorage.setItem(tokenKey, token);
        })
      );
  }
}
