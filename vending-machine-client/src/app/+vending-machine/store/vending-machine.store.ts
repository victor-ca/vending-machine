import { Product } from 'src/app/model/product';

export const vendingMachineFeatureName = 'vendingMachine';
const coins = [5, 10, 20, 50, 100] as const;
export type CoinKey = typeof coins[number];
export type CoinBank = Record<CoinKey, number>;
export type VendingMachineState = {
  purchaseInProcess: boolean;
  productsForSale: Product[];
  coinBank: CoinBank;
};
