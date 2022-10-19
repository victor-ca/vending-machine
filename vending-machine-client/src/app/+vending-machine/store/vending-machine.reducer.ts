import { createReducer, on } from '@ngrx/store';
import { insertCoinActions } from './vending-machine.actions';
import { CoinKey, VendingMachineState } from './vending-machine.store';

const initialState: VendingMachineState = {
  coinBank: {
    '5': 0,
    '10': 0,
    '100': 0,
    '20': 0,
    '50': 0,
  },
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
  }))
);
