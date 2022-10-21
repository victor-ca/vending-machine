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
import { NZ_I18N } from 'ng-zorro-antd/i18n';
import { en_US } from 'ng-zorro-antd/i18n';
import { registerLocaleData } from '@angular/common';
import en from '@angular/common/locales/en';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { IconsProviderModule } from './icons-provider.module';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { LoggedInUserComponent } from './auth/logged-in-user/logged-in-user.component';
import { AuthEffects } from './auth/store/auth.effects';
registerLocaleData(en);
@NgModule({
  declarations: [AppComponent, LoggedInUserComponent],
  imports: [
    BrowserModule,
    NzButtonModule,
    NgrxFormsModule,
    NzBreadCrumbModule,
    HttpClientModule,
    EffectsModule.forRoot([AuthEffects]),
    StoreModule.forRoot(appReducer),
    !environment.production
      ? StoreDevtoolsModule.instrument({
          name: 'VendingMachine',
          maxAge: 25,
        })
      : [],

    RouterModule.forRoot(appRoutes, { enableTracing: true }),
    FormsModule,
    BrowserAnimationsModule,
    IconsProviderModule,
    NzLayoutModule,
    NzMenuModule,
  ],
  providers: [
    { provide: API_ENDPOINT, useValue: environment.apiEndpoint },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: BearerAuthInterceptor,
      multi: true,
    },
    { provide: NZ_I18N, useValue: en_US },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
