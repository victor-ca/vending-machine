import { createAction, props } from '@ngrx/store';
import { CurrentUser } from 'src/app/auth/auth.service';

export const registerActions = {
  start: createAction(
    'register user start',
    props<{ userName: string; password: string; isSeller: boolean }>()
  ),
  success: createAction(
    'register user success',
    props<{ user: CurrentUser }>()
  ),
  failure: createAction('register user failure'),
};

export const loginUserActions = {
  start: createAction(
    'login user start',
    props<{ userName: string; password: string }>()
  ),
  success: createAction('login user success', props<{ user: CurrentUser }>()),
  failure: createAction('login user failure'),
};

export const logoutActions = {
  start: createAction('logout start'),
  success: createAction('log out success', props<{ user: CurrentUser }>()),
};
