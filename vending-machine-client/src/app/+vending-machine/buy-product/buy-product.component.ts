import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { Store } from '@ngrx/store';
import { combineLatest, filter, map, Observable } from 'rxjs';
import { Product } from 'src/app/model/product';
import {
  selectAvailableAmount,
  selectProductsForSale,
} from '../store/vending-machine.selectors';

@Component({
  selector: 'app-buy-product',
  templateUrl: './buy-product.component.html',
  styleUrls: ['./buy-product.component.scss'],
})
export class BuyProductComponent implements OnInit {
  amount: number = 1;
  product$!: Observable<Product | undefined>;
  availableAmount$!: Observable<number>;

  constructor(private store: Store, private activatedRoute: ActivatedRoute) {}

  ngOnInit(): void {
    this.availableAmount$ = this.store.select(selectAvailableAmount);

    const currentProductName$: Observable<string> =
      this.activatedRoute.params.pipe(
        map((p) => p['productName']),
        filter((p) => !!p)
      );
    this.product$ = combineLatest([
      this.store.select(selectProductsForSale),
      currentProductName$,
    ]).pipe(
      map(([all, name]) => {
        return all.find((p) => p.name == name);
      })
    );
  }
}
