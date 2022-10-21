import { Inject, Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { AuthService } from './auth.service';
import { API_ENDPOINT } from './endpoint';

@Injectable()
export class BearerAuthInterceptor implements HttpInterceptor {
  constructor(
    private readonly authService: AuthService,
    @Inject(API_ENDPOINT) private readonly apiEndpoint: string
  ) {}

  intercept(
    req: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    const isOnAuthRoute = req.url.startsWith(`${this.apiEndpoint}/auth`);
    if (isOnAuthRoute || req.url.indexOf(this.apiEndpoint) !== 0) {
      return next.handle(req);
    }

    return this.authService.getAuthToken().pipe(
      switchMap((token) => {
        return !!token
          ? next.handle(
              req.clone({
                headers: req.headers.set('Authorization', `Bearer ${token}`),
              })
            )
          : next.handle(req);
      })
    );
  }
}
