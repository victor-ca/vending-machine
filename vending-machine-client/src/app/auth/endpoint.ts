import { InjectionToken } from '@angular/core';

export const API_ENDPOINT = new InjectionToken<string>(
  'environment.apiEndpoint'
);
