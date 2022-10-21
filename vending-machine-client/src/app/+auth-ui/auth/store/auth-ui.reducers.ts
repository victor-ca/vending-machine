import { combineReducers, createReducer } from '@ngrx/store';
import {
  createFormGroupState,
  createFormStateReducerWithUpdate,
  onNgrxForms,
  updateGroup,
  validate,
} from 'ngrx-forms';
import { required } from 'ngrx-forms/validation';
import { AuthUiState, LoginForm, RegisterForm } from './auth-ui.state';

export const registerFormId = 'registerForm';
export const loginFormId = 'registerForm';

const initialRegisterFormState = createFormGroupState<RegisterForm>(
  registerFormId,
  {
    password: '',
    userName: '',
    isSeller: false,
  }
);

const initialLoginFormState = createFormGroupState<LoginForm>(loginFormId, {
  password: '',
  userName: '',
});

const validateLogin = updateGroup<LoginForm>({
  password: validate(required),
  userName: validate(required),
});
const validateRegister = updateGroup<RegisterForm>({
  password: validate(required),
  userName: validate(required),
});

export const authUiReducer = combineReducers<AuthUiState>(
  {
    loginForm: createFormStateReducerWithUpdate<LoginForm>(validateLogin),
    registerForm:
      createFormStateReducerWithUpdate<RegisterForm>(validateRegister),
  },
  { loginForm: initialLoginFormState, registerForm: initialRegisterFormState }
);
