import { createFeatureSelector, createSelector } from '@ngrx/store';
import { authUiFeatureName, AuthUiState } from './auth-ui.state';

const selectAuthUiState = createFeatureSelector<AuthUiState>(authUiFeatureName);

export const selectRegisterForm = createSelector(
  selectAuthUiState,
  (s) => s.registerForm
);
export const selectLoginForm = createSelector(
  selectAuthUiState,
  (s) => s.loginForm
);
