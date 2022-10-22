import { createAction, props } from '@ngrx/store';

export const successToastAction = createAction(
  'toast success',
  props<{ message: string }>()
);
