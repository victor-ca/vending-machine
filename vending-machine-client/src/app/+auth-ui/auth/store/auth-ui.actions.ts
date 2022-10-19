import { createAction, props } from '@ngrx/store';

export const registerActions = {
  start: createAction(
    'register user start',
    props<{ userName: string; password: string }>()
  ),
  success: createAction('register user success'),
  failure: createAction('register user failure'),
};
export const loginUserActions = {
  start: createAction(
    'login user start',
    props<{ userName: string; password: string }>()
  ),
  success: createAction('login user success'),
  failure: createAction('login user failure'),
};
