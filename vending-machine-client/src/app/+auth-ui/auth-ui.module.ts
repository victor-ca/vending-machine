import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegisterComponent } from './auth/register/register.component';
import { LoginComponent } from './auth/login/login.component';
import { RouterModule } from '@angular/router';
import { authRoutes } from './auth-ui.routes';
import { NgrxFormsModule } from 'ngrx-forms';
import { StoreModule } from '@ngrx/store';
import { authUiFeatureName } from './auth/store/auth-ui.state';
import { authUiReducer } from './auth/store/auth-ui.reducers';
import { AuthUiEffects } from './auth/store/auth-ui.effects';
import { EffectsModule } from '@ngrx/effects';

@NgModule({
  declarations: [LoginComponent, RegisterComponent],
  imports: [
    CommonModule,
    NgrxFormsModule,
    EffectsModule.forFeature([AuthUiEffects]),
    StoreModule.forFeature(authUiFeatureName, authUiReducer),
    RouterModule.forChild(authRoutes),
  ],
})
export class AuthUiModule {}
