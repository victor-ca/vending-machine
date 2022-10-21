import { createFeatureSelector, createSelector } from '@ngrx/store';
import { CoinStack } from '../coin-stack';
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

export const selectCoinStacks = createSelector(selectCoinBank, (bank) =>
  [...Object.entries(bank)].map(
    ([value, count]): CoinStack => ({ denomination: +value, count })
  )
);
export const selectAvailableAmount = createSelector(
  selectCoinStacks,
  (coinStacks) => {
    const cents = coinStacks
      .map((x) => x.count * x.denomination)
      .reduce((acc, next) => acc + next, 0);

    return cents / 100;
  }
);
