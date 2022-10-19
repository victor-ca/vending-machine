import { Action, createReducer } from '@ngrx/store';
import { createFormGroupState, FormGroupState } from 'ngrx-forms';

export type RegisterForm = {
  userName: string;
  password: string;
};
export type LoginForm = {
  userName: string;
  password: string;
};
export const authUiFeatureName = 'authUi';

export type AuthUiState = {
  registerForm: FormGroupState<RegisterForm>;
  loginForm: FormGroupState<LoginForm>;
};
