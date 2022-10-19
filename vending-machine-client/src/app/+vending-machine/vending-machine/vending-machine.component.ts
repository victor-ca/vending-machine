import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { Product } from 'src/app/model/product';
import {
  selectCoinBank,
  selectProductsForSale,
} from '../store/vending-machine.selectors';
import { CoinBank } from '../store/vending-machine.store';

@Component({
  selector: 'app-vending-machine',
  templateUrl: './vending-machine.component.html',
  styleUrls: ['./vending-machine.component.scss'],
})
export class VendingMachineComponent implements OnInit {
  productsForSale$!: Observable<Product[]>;
  coinBank$!: Observable<CoinBank>;
  constructor(private store: Store) {}

  ngOnInit(): void {
    this.productsForSale$ = this.store.select(selectProductsForSale);
    this.coinBank$ = this.store.select(selectCoinBank);
  }
}
