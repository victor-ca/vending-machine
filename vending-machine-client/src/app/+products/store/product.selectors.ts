import { createFeatureSelector, createSelector } from '@ngrx/store';
import { productsFeatureName, ProductsState } from './products.store';

const productsState = createFeatureSelector<ProductsState>(productsFeatureName);

export const selectOwnedProducts = createSelector(
  productsState,
  (s) => s.ownedProducts
);
export const selectNewProductForm = createSelector(
  productsState,
  (s) => s.newProductForm
);
