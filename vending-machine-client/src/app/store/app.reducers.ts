import { createReducer } from '@ngrx/store';
import { defaultAppState } from './app.state';

export const appReducer = createReducer(defaultAppState);
