import { Injectable } from '@angular/core';
import { Actions, createEffect } from '@ngrx/effects';
import { NzNotificationService } from 'ng-zorro-antd/notification';

import { filter, tap } from 'rxjs';

@Injectable()
export class AppNotificationsEffects {
  loadProductsForSale$ = createEffect(
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
