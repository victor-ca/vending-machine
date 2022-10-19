import { AuthService } from 'src/app/auth/auth.service';
import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';

import { map, switchMap } from 'rxjs';
import {
  createNewProductActions,
  loadOwnedProductsActions,
  setProductAmountActions,
} from './product.actions';
import { ProductsServiceService as ProductsService } from '../products.service';

@Injectable()
export class OwnedProductsEffects {
  loadOwned$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadOwnedProductsActions.start),
      switchMap(() =>
        this.productsService
          .getOwned()
          .pipe(
            map((products) => loadOwnedProductsActions.success({ products }))
          )
      )
    )
  );
  createNew$ = createEffect(() =>
    this.actions$.pipe(
      ofType(createNewProductActions.start),
      switchMap(({ product }) =>
        this.productsService
          .createProduct(product)
          .pipe(map((product) => createNewProductActions.success({ product })))
      )
    )
  );

  setAmount$ = createEffect(() =>
    this.actions$.pipe(
      ofType(setProductAmountActions.start),
      switchMap(({ amount, productName }) =>
        this.productsService
          .setProductAmount(productName, amount)
          .pipe(map((product) => setProductAmountActions.success({ product })))
      )
    )
  );

  constructor(
    private readonly actions$: Actions,
    private readonly productsService: ProductsService
  ) {}
}
