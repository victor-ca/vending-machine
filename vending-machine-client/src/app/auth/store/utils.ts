import { ActionCreator } from '@ngrx/store';
import { Observable, of } from 'rxjs';

export const createErrorAction =
  <C extends ActionCreator>(
    actionCreator: C,
    additionalProps?: Partial<ReturnType<C>>
  ): ((err: Error | string) => Observable<ReturnType<C>>) =>
  (error) => {
    console.warn(error);
    const action = actionCreator(additionalProps);
    return of(action) as any;
  };
