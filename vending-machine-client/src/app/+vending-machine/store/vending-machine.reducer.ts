import { createReducer } from '@ngrx/store';
import { VendingMachineState } from './vending-machine.store';

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

export const vendingMachineReducer =
  createReducer<VendingMachineState>(initialState);
