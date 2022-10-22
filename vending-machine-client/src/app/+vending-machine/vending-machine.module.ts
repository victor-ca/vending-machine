import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VendingMachineComponent } from './vending-machine/vending-machine.component';
import { StoreModule } from '@ngrx/store';
import { vendingMachineFeatureName } from './store/vending-machine.store';
import { vendingMachineReducer } from './store/vending-machine.reducer';
import { EffectsModule } from '@ngrx/effects';
import { VendingMachineEffects } from './store/vending-machine.effects';
import { RouterModule } from '@angular/router';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { CoinBankComponent } from './coin-bank/coin-bank.component';
import { BuyProductComponent } from './buy-product/buy-product.component';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzNotificationModule } from 'ng-zorro-antd/notification';
@NgModule({
  declarations: [
    VendingMachineComponent,
    CoinBankComponent,
    BuyProductComponent,
  ],
  imports: [
    CommonModule,
    NzTableModule,
    NzButtonModule,
    NzInputModule,
    NzNotificationModule,
    NzDividerModule,
    EffectsModule.forFeature([VendingMachineEffects]),
    StoreModule.forFeature(vendingMachineFeatureName, vendingMachineReducer),
    RouterModule.forChild([
      {
        path: '',
        component: VendingMachineComponent,
        children: [
          {
            path: 'coins',
            component: CoinBankComponent,
          },
          {
            path: 'buy/:productName',
            component: BuyProductComponent,
          },
          {
            path: '**',
            redirectTo: 'coins',
          },
        ],
      },
    ]),
  ],
})
export class VendingMachineModule {}
