import { AuthService } from 'src/app/auth/auth.service';
import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';

import { catchError, concatMap, map, mergeMap, switchMap } from 'rxjs';
import {
  createNewProductActions,
  deleteProductActions,
  loadOwnedProductsActions,
  updateProductActions,
} from './product.actions';
import { ProductsServiceService as ProductsService } from '../products.service';
import { createErrorAction } from 'src/app/auth/store/utils';
import { successToastAction } from 'src/app/store/app.actions';

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
      concatMap(({ product }) =>
        this.productsService.createProduct(product).pipe(
          map((product) => createNewProductActions.success({ product })),
          catchError(createErrorAction(createNewProductActions.failure))
        )
      )
    )
  );

  delete$ = createEffect(() =>
    this.actions$.pipe(
      ofType(deleteProductActions.start),
      concatMap(({ productName }) =>
        this.productsService
          .deleteProduct(productName)
          .pipe(map((product) => createNewProductActions.success({ product })))
      )
    )
  );

  setAmount$ = createEffect(() =>
    this.actions$.pipe(
      ofType(updateProductActions.start),
      concatMap(({ originalName, update }) =>
        this.productsService.updateProduct(originalName, update).pipe(
          map((product) =>
            updateProductActions.success({
              originalName,
              updatedProduct: product,
            })
          ),
          catchError(createErrorAction(updateProductActions.failure))
        )
      )
    )
  );

  confirmUpdate$ = createEffect(() =>
    this.actions$.pipe(
      ofType(updateProductActions.success),
      map(() => successToastAction({ message: 'product create successfully' }))
    )
  );

  constructor(
    private readonly actions$: Actions,
    private readonly productsService: ProductsService
  ) {}
}
