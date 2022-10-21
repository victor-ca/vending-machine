import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';

import { concatMap, map, switchMap } from 'rxjs';
import { loadOwnedProductsActions } from 'src/app/+products/store/product.actions';
import { VendingMachineService } from '../vending-machine.service';
import {
  insertCoinActions,
  loadExistingCoins,
  loadProductsForSaleActions,
  purchaseProductActions,
} from './vending-machine.actions';

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

  loadProductsForSale$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadProductsForSaleActions.start),
      switchMap(() => this.vendingMachineService.loadProducts()),
      map((products) => loadProductsForSaleActions.success({ products }))
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
        this.vendingMachineService.purchase(productName, desiredAmount)
      ),
      map((coins) => purchaseProductActions.success({ coins }))
    )
  );

  constructor(
    private readonly actions$: Actions,
    private readonly vendingMachineService: VendingMachineService
  ) {}
}
