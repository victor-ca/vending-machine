import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { combineLatest, filter, map, Observable } from 'rxjs';
import { Product } from 'src/app/model/product';
import { setProductAmountActions } from '../../store/product.actions';
import { selectOwnedProducts } from '../../store/product.selectors';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss'],
})
export class ProductDetailsComponent implements OnInit {
  currentProduct$!: Observable<Product>;
  constructor(private store: Store, private activatedRoute: ActivatedRoute) {}

  ngOnInit(): void {
    const currentProductName$ = this.activatedRoute.params.pipe(
      map((p) => p['productId']),
      filter((p) => !!p)
    );
    this.currentProduct$ = combineLatest([
      this.store.select(selectOwnedProducts),
      currentProductName$,
    ]).pipe(
      map(([all, name]) => {
        return all.find((x) => x.name == name)!;
      })
    );
  }
  updateAvailableAmount(productName: string, amount: number) {
    this.store.dispatch(setProductAmountActions.start({ productName, amount }));
  }
}
