import { FormGroupState } from 'ngrx-forms';
import { Product } from 'src/app/model/product';

export type NewProductForm = Product;
export type ProductsState = {
  ownedProducts: Product[];
  newProductForm: FormGroupState<NewProductForm>;
};
export const productsFeatureName = 'products';
