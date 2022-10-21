import { Route } from '@angular/router';
import { NewProductComponent } from './owned-products-list/new-product/new-product.component';
import { OwnedProductsListComponent } from './owned-products-list/owned-products-list.component';
import { ProductDetailsComponent } from './owned-products-list/product-details/product-details.component';

export const productRoutes: Route[] = [
  {
    path: '',
    component: OwnedProductsListComponent,
    children: [
      {
        path: 'new',
        component: NewProductComponent,
      },
      {
        path: ':productName',
        component: ProductDetailsComponent,
      },
      { path: '**', redirectTo: 'new' },
    ],
  },
];
