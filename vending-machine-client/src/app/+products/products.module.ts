import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OwnedProductsListComponent } from './owned-products-list/owned-products-list.component';
import { RouterModule } from '@angular/router';
import { productRoutes } from './product.routes';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { productsFeatureName } from './store/products.store';
import { productsReducer } from './store/product.reducers';
import { OwnedProductsEffects } from './store/product.effects';
import { NewProductComponent } from './owned-products-list/new-product/new-product.component';
import { ProductDetailsComponent } from './owned-products-list/product-details/product-details.component';
import { NgrxFormsModule } from 'ngrx-forms';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzDividerModule } from 'ng-zorro-antd/divider';

@NgModule({
  declarations: [
    OwnedProductsListComponent,
    NewProductComponent,
    ProductDetailsComponent,
  ],
  imports: [
    CommonModule,
    NzTableModule,
    NzDividerModule,
    NgrxFormsModule,
    RouterModule.forChild(productRoutes),
    EffectsModule.forFeature([OwnedProductsEffects]),
    StoreModule.forFeature(productsFeatureName, productsReducer),
  ],
})
export class ProductsModule {}
