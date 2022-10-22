import { createAction, props } from '@ngrx/store';
import { Product } from 'src/app/model/product';
import { PurchaseResult } from '../service/purchase-result';
import { CoinBank } from './vending-machine.store';

export const insertCoinActions = {
  start: createAction(
    'insert coin start',
    props<{
      denomination: number;
    }>()
  ),
  success: createAction('insert coin success'),
};

export const loadProductsForSaleActions = {
  start: createAction('load products for sale start'),
  success: createAction(
    'load products for sale success',
    props<{ products: Product[] }>()
  ),
  failure: createAction('load products for sale failure'),
};

export const loadExistingCoins = {
  start: createAction('load existing coins start'),
  success: createAction(
    'load existing coins success',
    props<{ coins: CoinBank }>()
  ),
  failure: createAction('load existing coins failure'),
};

export const purchaseProductActions = {
  start: createAction(
    'purchase product start',
    props<{ productName: string; desiredAmount: number }>()
  ),
  success: createAction(
    'purchase product success',
    props<{
      result: PurchaseResult;
      productName: string;
      desiredAmount: number;
    }>()
  ),
  failure: createAction('purchase product failure'),
};
