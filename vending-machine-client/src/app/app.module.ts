import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { AppComponent } from './app.component';

import { StoreModule } from '@ngrx/store';
import { NgrxFormsModule } from 'ngrx-forms';
import { appReducer } from './store/app.reducers';
import { RouterModule } from '@angular/router';
import { appRoutes } from './app.routes';
import { API_ENDPOINT } from './auth/endpoint';
import { environment } from 'src/environments/environment';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { EffectsModule } from '@ngrx/effects';
import { BearerAuthInterceptor } from './auth/http-auth.interceptor';
@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    NgrxFormsModule,
    HttpClientModule,
    EffectsModule.forRoot(),
    StoreModule.forRoot(appReducer),
    !environment.production
      ? StoreDevtoolsModule.instrument({
          name: 'VendingMachine',
          maxAge: 25,
        })
      : [],

    RouterModule.forRoot(appRoutes),
  ],
  providers: [
    { provide: API_ENDPOINT, useValue: environment.apiEndpoint },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: BearerAuthInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
