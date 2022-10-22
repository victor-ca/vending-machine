import { createReducer, on } from '@ngrx/store';
import {
  insertCoinActions,
  loadExistingCoins,
  loadProductsForSaleActions,
  purchaseProductActions,
} from './vending-machine.actions';
import { CoinKey, VendingMachineState } from './vending-machine.store';

const emptyCoinBank = { '5': 0, '10': 0, '100': 0, '20': 0, '50': 0 };
const initialState: VendingMachineState = {
  purchaseInProcess: false,
  coinBank: emptyCoinBank,
  productsForSale: [],
};

export const vendingMachineReducer = createReducer<VendingMachineState>(
  initialState,
  on(insertCoinActions.start, (s, { denomination }) => ({
    ...s,
    coinBank: {
      ...s.coinBank,
      [denomination]: s.coinBank[denomination as CoinKey] + 1,
    },
  })),
  on(purchaseProductActions.start, (s, a) => ({
    ...s,
    purchaseInProcess: true,
  })),
  on(purchaseProductActions.success, purchaseProductActions.failure, (s) => ({
    ...s,
    purchaseInProcess: false,
  })),
  on(purchaseProductActions.success, (s, a) => ({
    ...s,
    productsForSale: s.productsForSale.map((product) =>
      product.name === a.productName
        ? {
            ...product,
            amountAvailable: product.amountAvailable - a.desiredAmount,
          }
        : product
    ),
    coinBank: {
      ...emptyCoinBank,
      ...a.result.newCoinBankState,
    },
  })),
  on(loadExistingCoins.success, (s, { coins }) => ({
    ...s,
    coinBank: {
      ...emptyCoinBank,
      ...coins,
    },
  })),
  on(loadProductsForSaleActions.success, (s, { products }) => ({
    ...s,
    productsForSale: products,
  }))
);
