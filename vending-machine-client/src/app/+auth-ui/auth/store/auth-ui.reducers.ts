import { createReducer } from '@ngrx/store';
import { createFormGroupState, onNgrxForms } from 'ngrx-forms';
import { AuthUiState, LoginForm, RegisterForm } from './auth-ui.state';

export const registerFormId = 'registerForm';
export const loginFormId = 'registerForm';

const initialRegisterFormState = createFormGroupState<RegisterForm>(
  registerFormId,
  {
    password: '',
    userName: '',
  }
);

const initialLoginFormState = createFormGroupState<LoginForm>(loginFormId, {
  password: '',
  userName: '',
});

export const authUiReducer = createReducer<AuthUiState>(
  { registerForm: initialRegisterFormState, loginForm: initialLoginFormState },
  onNgrxForms()
);
