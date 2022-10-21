import { Route } from '@angular/router';
import { AppComponent } from './app.component';
import { AuthBumperGuard } from './auth/guards/auth-bump.guard';

export const appRoutes: Route[] = [
  {
    path: 'auth',
    canActivate: [AuthBumperGuard],
    loadChildren: () =>
      import('./+auth-ui/auth-ui.module').then((m) => m.AuthUiModule),
  },
  {
    path: 'seller',
    canActivate: [AuthBumperGuard],
    loadChildren: () =>
      import('./+products/products.module').then((m) => m.ProductsModule),
  },
  {
    path: 'store',
    canActivate: [AuthBumperGuard],
    loadChildren: () =>
      import('./+vending-machine/vending-machine.module').then(
        (m) => m.VendingMachineModule
      ),
  },
  {
    path: '**',
    canActivate: [AuthBumperGuard],
    component: AppComponent,
  },
];
