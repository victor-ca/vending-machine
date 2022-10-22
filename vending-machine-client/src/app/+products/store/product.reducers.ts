import { createReducer, on } from '@ngrx/store';
import { createFormGroupState, onNgrxForms } from 'ngrx-forms';
import {
  createNewProductActions,
  deleteProductActions,
  loadOwnedProductsActions,
  randomizeNewProductFormAction,
  updateProductActions,
} from './product.actions';
import { NewProductForm, ProductsState } from './products.store';
import {
  uniqueNamesGenerator,
  adjectives,
  animals,
} from 'unique-names-generator';

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
  on(updateProductActions.success, (s, { originalName, updatedProduct }) => ({
    ...s,
    ownedProducts: s.ownedProducts.map((x) =>
      x.name == originalName ? updatedProduct : x
    ),
  })),
  on(deleteProductActions.start, (s, { productName }) => ({
    ...s,
    ownedProducts: s.ownedProducts.filter((x) => x.name != productName),
  })),
  on(randomizeNewProductFormAction, (s) => ({
    ...s,
    newProductForm: createFormGroupState<NewProductForm>(
      newProductFormId,
      getRandomProduct()
    ),
  })),
  on(createNewProductActions.success, (s, { product }) => ({
    ...s,
    ownedProducts: [...s.ownedProducts, product],
  }))
);

const getRandomProduct = (): NewProductForm => ({
  amountAvailable: 1 + Math.round(Math.random() * 10),
  cost: Math.round(5 + Math.round(Math.random() * 50) * 5) / 100,
  name: uniqueNamesGenerator({
    dictionaries: [adjectives, animals],
    separator: ' ',
  }),
});
