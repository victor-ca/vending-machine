import { Route } from '@angular/router';

export const appRoutes: Route[] = [
  {
    path: 'auth',
    loadChildren: () =>
      import('./+auth-ui/auth-ui.module').then((m) => m.AuthUiModule),
  },
  {
    path: 'products',
    loadChildren: () =>
      import('./+products/products.module').then((m) => m.ProductsModule),
  },
];
