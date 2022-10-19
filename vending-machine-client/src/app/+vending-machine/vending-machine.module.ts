import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VendingMachineComponent } from './vending-machine/vending-machine.component';
import { StoreModule } from '@ngrx/store';
import { vendingMachineFeatureName } from './store/vending-machine.store';
import { vendingMachineReducer } from './store/vending-machine.reducer';
import { EffectsModule } from '@ngrx/effects';
import { VendingMachineEffects } from './store/vending-machine.effects';
import { Router, RouterModule } from '@angular/router';

@NgModule({
  declarations: [VendingMachineComponent],
  imports: [
    CommonModule,
    EffectsModule.forFeature([VendingMachineEffects]),
    StoreModule.forFeature(vendingMachineFeatureName, vendingMachineReducer),
    RouterModule.forChild([
      {
        path: '',
        component: VendingMachineComponent,
      },
    ]),
  ],
})
export class VendingMachineModule {}
