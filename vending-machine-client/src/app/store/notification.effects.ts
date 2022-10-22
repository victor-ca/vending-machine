import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { NzNotificationService } from 'ng-zorro-antd/notification';

import { filter, tap } from 'rxjs';
import { successToastAction } from './app.actions';

@Injectable()
export class AppNotificationsEffects {
  successToast$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(successToastAction),
        tap(({ message }) =>
          this.notificationService.success('Success', message)
        )
      ),
    { dispatch: false }
  );

  failActionToast$ = createEffect(
    () =>
      this.actions$.pipe(
        filter((a) => a.type.includes('failure')),
        tap((a) => this.notificationService.error('Error', a.type))
      ),
    { dispatch: false }
  );

  constructor(
    private readonly actions$: Actions,
    private notificationService: NzNotificationService
  ) {}
}
