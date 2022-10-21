import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { CoinStack } from '../coin-stack';
import { insertCoinActions } from '../store/vending-machine.actions';
import {
  selectAvailableAmount,
  selectCoinStacks,
} from '../store/vending-machine.selectors';

@Component({
  selector: 'app-coin-bank',
  templateUrl: './coin-bank.component.html',
  styleUrls: ['./coin-bank.component.scss'],
})
export class CoinBankComponent implements OnInit {
  coinStacks$!: Observable<CoinStack[]>;
  availableAmount$!: Observable<number>;

  constructor(private store: Store) {}
  ngOnInit(): void {
    this.coinStacks$ = this.store.select(selectCoinStacks);

    this.availableAmount$ = this.store.select(selectAvailableAmount);
  }
  insertCoin(denomination: number) {
    this.store.dispatch(insertCoinActions.start({ denomination }));
  }
}
