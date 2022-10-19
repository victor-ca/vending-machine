import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { Product } from 'src/app/model/product';
import { loadOwnedProductsActions } from '../store/product.actions';
import { selectOwnedProducts } from '../store/product.selectors';

@Component({
  selector: 'app-owned-products-list',
  templateUrl: './owned-products-list.component.html',
  styleUrls: ['./owned-products-list.component.scss'],
})
export class OwnedProductsListComponent implements OnInit {
  ownedProducts$!: Observable<Product[]>;
  constructor(private store: Store) {}

  ngOnInit(): void {
    this.store.dispatch(loadOwnedProductsActions.start());
    this.ownedProducts$ = this.store.select(selectOwnedProducts);
  }
}
