import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';

import { map, switchMap } from 'rxjs';
import { VendingMachineService } from '../vending-machine.service';
import { insertCoinActions } from './vending-machine.actions';

@Injectable()
export class VendingMachineEffects {
  insertCoin$ = createEffect(() =>
    this.actions$.pipe(
      ofType(insertCoinActions.start),
      switchMap(({ denomination }) =>
        this.vendingMachineService.insertCoin(denomination)
      ),
      map(() => insertCoinActions.success())
    )
  );

  constructor(
    private readonly actions$: Actions,
    private readonly vendingMachineService: VendingMachineService
  ) {}
}
