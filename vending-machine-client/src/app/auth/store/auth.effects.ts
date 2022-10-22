import { AuthService } from 'src/app/auth/auth.service';
import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import {
  loginUserActions,
  logoutActions,
  registerActions,
} from './auth.actions';
import { catchError, map, Observable, of, switchMap, tap } from 'rxjs';
import { Router } from '@angular/router';
import { autoNavigateAwayIfRequired } from 'src/app/store/auto-navigate';
import { ActionCreator } from '@ngrx/store';
import { Action, TypedAction } from '@ngrx/store/src/models';
import { createErrorAction } from './utils';

@Injectable()
export class AuthEffects {
  register$ = createEffect(() =>
    this.actions$.pipe(
      ofType(registerActions.start),
      switchMap(({ password, userName, isSeller }) =>
        this.authService.register(userName, password, isSeller).pipe(
          map((user) => registerActions.success({ user })),
          catchError(createErrorAction(registerActions.failure))
        )
      )
    )
  );
  login$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loginUserActions.start),
      switchMap(({ password, userName }) =>
        this.authService.login(userName, password).pipe(
          map((user) => loginUserActions.success({ user })),
          catchError(createErrorAction(loginUserActions.failure))
        )
      )
    )
  );

  logOut$ = createEffect(() =>
    this.actions$.pipe(
      ofType(
        logoutActions.start,
        loginUserActions.failure,
        registerActions.failure
      ),
      switchMap(() => this.authService.logOut()),
      map((user) => logoutActions.success({ user }))
    )
  );

  autoNav$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(
          loginUserActions.success,
          registerActions.success,
          logoutActions.success
        ),
        tap(({ user }) => autoNavigateAwayIfRequired(user, this.router))
      ),
    { dispatch: false }
  );

  constructor(
    private readonly actions$: Actions,
    private readonly authService: AuthService,
    private router: Router
  ) {}
}
