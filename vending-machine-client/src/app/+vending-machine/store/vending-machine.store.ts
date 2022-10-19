import { Product } from 'src/app/model/product';

export const vendingMachineFeatureName = 'vendingMachine';
const coins = [5, 10, 20, 50, 100] as const;
export type CoinBank = Record<typeof coins[number], number>;
export type VendingMachineState = {
  productsForSale: Product[];
  coinBank: CoinBank;
};
