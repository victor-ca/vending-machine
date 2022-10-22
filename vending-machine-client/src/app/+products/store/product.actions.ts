import { createAction, props } from '@ngrx/store';
import { Product } from 'src/app/model/product';

export const loadOwnedProductsActions = {
  start: createAction('load owned products'),
  success: createAction(
    'load owned products success',
    props<{ products: Product[] }>()
  ),
  failure: createAction(
    'load owned products failure',
    props<{ products: Product[] }>()
  ),
};

export const deleteProductActions = {
  start: createAction('delete product', props<{ productName: string }>()),
  success: createAction('delete products success'),
  failure: createAction('delete products failure'),
};

export const createNewProductActions = {
  start: createAction('create product', props<{ product: Product }>()),
  success: createAction(
    'create product success',
    props<{ product: Product }>()
  ),
  failure: createAction('create product failure'),
};

export const updateProductActions = {
  start: createAction(
    'set product amount start',
    props<{ originalName: string; update: Product }>()
  ),
  success: createAction(
    'set product amount success',
    props<{ originalName: string; updatedProduct: Product }>()
  ),
  failure: createAction('update product failure'),
};

export const randomizeNewProductFormAction = createAction(
  'randomize New Product Form'
);
