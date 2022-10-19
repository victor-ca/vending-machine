import { createReducer, on } from '@ngrx/store';
import { createFormGroupState, onNgrxForms } from 'ngrx-forms';
import {
  createNewProductActions,
  loadOwnedProductsActions,
  setProductAmountActions,
} from './product.actions';
import { NewProductForm, ProductsState } from './products.store';

export const newProductFormId = 'newProductForm';
export const defaultProductState: ProductsState = {
  ownedProducts: [],
  newProductForm: createFormGroupState<NewProductForm>(newProductFormId, {
    amountAvailable: 1,
    cost: 10,
    name: 'test',
  }),
};

export const productsReducer = createReducer<ProductsState>(
  defaultProductState,
  onNgrxForms(),
  on(loadOwnedProductsActions.success, (s, { products }) => ({
    ...s,
    ownedProducts: products,
  })),
  on(setProductAmountActions.success, (s, { product }) => ({
    ...s,
    ownedProducts: s.ownedProducts.map((x) =>
      x.name == product.name ? product : x
    ),
  })),
  on(createNewProductActions.success, (s, { product }) => ({
    ...s,
    ownedProducts: [...s.ownedProducts, product],
  }))
);
