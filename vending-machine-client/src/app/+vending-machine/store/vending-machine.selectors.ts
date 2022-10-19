import { createFeatureSelector, createSelector } from '@ngrx/store';
import {
  vendingMachineFeatureName,
  VendingMachineState,
} from './vending-machine.store';

const selectVendingMachineFeature = createFeatureSelector<VendingMachineState>(
  vendingMachineFeatureName
);

export const selectProductsForSale = createSelector(
  selectVendingMachineFeature,
  (s) => s.productsForSale
);

export const selectCoinBank = createSelector(
  selectVendingMachineFeature,
  (s) => s.coinBank
);
