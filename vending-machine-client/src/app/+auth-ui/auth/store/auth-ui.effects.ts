import { AuthService } from 'src/app/auth/auth.service';
import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { loginUserActions, registerActions } from './auth-ui.actions';
import { map, switchMap } from 'rxjs';

@Injectable()
export class AuthUiEffects {
  register$ = createEffect(() =>
    this.actions$.pipe(
      ofType(registerActions.start),
      switchMap(({ password, userName: username }) =>
        this.authService
          .register(username, password)
          .pipe(map(() => registerActions.success()))
      )
    )
  );
  login$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loginUserActions.start),
      switchMap(({ password, userName: username }) =>
        this.authService
          .login(username, password)
          .pipe(map(() => loginUserActions.success()))
      )
    )
  );

  constructor(
    private readonly actions$: Actions,
    private readonly authService: AuthService
  ) {}
}
