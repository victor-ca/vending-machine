import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { map, Observable } from 'rxjs';
import { Product } from 'src/app/model/product';
import { insertCoinActions } from '../store/vending-machine.actions';
import {
  selectCoinBank,
  selectProductsForSale,
} from '../store/vending-machine.selectors';
import { CoinBank } from '../store/vending-machine.store';

type CoinStack = {
  denomination: number;
  count: number;
};
@Component({
  selector: 'app-vending-machine',
  templateUrl: './vending-machine.component.html',
  styleUrls: ['./vending-machine.component.scss'],
})
export class VendingMachineComponent implements OnInit {
  productsForSale$!: Observable<Product[]>;
  coinStacks$!: Observable<CoinStack[]>;
  availableAmount$!: Observable<number>;
  constructor(private store: Store) {}

  ngOnInit(): void {
    this.productsForSale$ = this.store.select(selectProductsForSale);
    this.coinStacks$ = this.store
      .select(selectCoinBank)
      .pipe(
        map((bank) =>
          [...Object.entries(bank)].map(
            ([value, count]): CoinStack => ({ denomination: +value, count })
          )
        )
      );
    this.availableAmount$ = this.coinStacks$.pipe(
      map((coinStacks) => coinStacks.map((x) => x.count * x.denomination)),
      map((amounts) => amounts.reduce((acc, next) => acc + next, 0))
    );
  }

  insertCoin(denomination: number) {
    this.store.dispatch(insertCoinActions.start({ denomination }));
  }
}
