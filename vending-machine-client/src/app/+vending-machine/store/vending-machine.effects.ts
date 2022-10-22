import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { NzNotificationService } from 'ng-zorro-antd/notification';

import {
  catchError,
  concatMap,
  exhaustMap,
  map,
  of,
  switchMap,
  tap,
} from 'rxjs';
import { createErrorAction } from 'src/app/auth/store/utils';
import { VendingMachineService } from '../service/vending-machine.service';
import {
  insertCoinActions,
  loadExistingCoins,
  loadProductsForSaleActions,
  purchaseProductActions,
  resetCoinActions,
} from './vending-machine.actions';
import { CoinBank } from './vending-machine.store';

@Injectable()
export class VendingMachineEffects {
  insertCoin$ = createEffect(() =>
    this.actions$.pipe(
      ofType(insertCoinActions.start),
      concatMap(({ denomination }) =>
        this.vendingMachineService.insertCoin(denomination)
      ),
      map(() => insertCoinActions.success())
    )
  );

  resetCoins$ = createEffect(() =>
    this.actions$.pipe(
      ofType(resetCoinActions.start),
      exhaustMap(() => this.vendingMachineService.reset()),
      map(() => insertCoinActions.success())
    )
  );

  loadProductsForSale$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadProductsForSaleActions.start),
      switchMap(() => this.vendingMachineService.loadProducts()),
      map((products) => loadProductsForSaleActions.success({ products })),
      catchError(createErrorAction(loadProductsForSaleActions.failure))
    )
  );

  loadCoins$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadExistingCoins.start),
      switchMap(() => this.vendingMachineService.getBank()),
      map((coins) => loadExistingCoins.success({ coins }))
    )
  );

  purchase$ = createEffect(() =>
    this.actions$.pipe(
      ofType(purchaseProductActions.start),
      switchMap(({ desiredAmount, productName }) =>
        this.vendingMachineService.purchase(productName, desiredAmount).pipe(
          map((result) =>
            purchaseProductActions.success({
              result,
              desiredAmount,
              productName,
            })
          ),
          catchError(() => of(purchaseProductActions.failure()))
        )
      )
    )
  );

  purchaseNotification$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(purchaseProductActions.success),
        tap(({ desiredAmount, productName, result }) => {
          this.notificationService.blank(
            'Purchase Successful',
            [
              `Successfully bought ${desiredAmount} x [${productName}] ($${
                result.purchaseAmountInCents / 100
              })`,
              `spending in coins $${result.actualSpentInCents / 100}`,
              `using ${printCoins(result.usedCoins)}`,
              `change: $${result.changeAmountInCents / 100}`,
              result.changeAmountInCents &&
                `returned using ${printCoins(result.changeCoins)}`,
            ]
              .filter(Boolean)
              .join('\n'),
            {
              nzDuration: 0,
              nzPlacement: 'bottomRight',
              nzStyle: { whiteSpace: 'pre' },
            }
          );
        })
      ),
    { dispatch: false }
  );

  constructor(
    private readonly actions$: Actions,
    private readonly vendingMachineService: VendingMachineService,
    private readonly notificationService: NzNotificationService
  ) {}
}

function printCoins(usedCoins: CoinBank) {
  return Object.entries(usedCoins)
    .filter(([_denomination, count]) => count !== 0)
    .map(([denomination, count]) => `${count} x ${denomination}¢`)
    .join(' + ');
}
