import { CoinBank } from '../store/vending-machine.store';

export type PurchaseResult = {
  purchaseAmountInCents: number;
  actualSpentInCents: number;
  changeAmountInCents: number;
  usedCoins: CoinBank;
  newCoinBankState: CoinBank;
  changeCoins: CoinBank;
};
