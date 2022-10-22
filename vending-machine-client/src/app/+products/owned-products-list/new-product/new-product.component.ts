import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { FormGroupState, setValue } from 'ngrx-forms';
import { Observable, take } from 'rxjs';
import {
  createNewProductActions,
  randomizeNewProductFormAction,
} from '../../store/product.actions';
import { selectNewProductForm } from '../../store/product.selectors';
import { NewProductForm } from '../../store/products.store';

@Component({
  selector: 'app-new-product',
  templateUrl: './new-product.component.html',
  styleUrls: ['./new-product.component.scss'],
})
export class NewProductComponent implements OnInit {
  formState$!: Observable<FormGroupState<NewProductForm>>;
  constructor(private readonly store: Store) {
    this.formState$ = this.store.select(selectNewProductForm);
  }
  ngOnInit(): void {
    this.store.dispatch(randomizeNewProductFormAction());
  }

  create() {
    this.formState$.pipe(take(1)).subscribe(({ controls }) => {
      const { amountAvailable, cost, name } = controls;
      this.store.dispatch(
        createNewProductActions.start({
          product: {
            amountAvailable: amountAvailable.value,
            cost: cost.value,
            name: name.value,
          },
        })
      );
    });

    this.store.dispatch(randomizeNewProductFormAction());
  }
}
