import { createAction, props } from '@ngrx/store';

export const insertCoinActions = {
  start: createAction(
    'insert coin start',
    props<{
      denomination: number;
    }>()
  ),
  success: createAction('insert coin success'),
};
