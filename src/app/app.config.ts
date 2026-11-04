import {
  ApplicationConfig,
  importProvidersFrom,
  provideBrowserGlobalErrorListeners,
} from '@angular/core';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideRouter, withInMemoryScrolling } from '@angular/router';

import { JwtModule } from '@auth0/angular-jwt';

import { authInterceptor } from './core/auth/interceptors/auth.interceptor';
import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes, withInMemoryScrolling({ scrollPositionRestoration: 'top' })),
    provideHttpClient(withInterceptors([authInterceptor])),

    importProvidersFrom(
      JwtModule.forRoot({
        config: { tokenGetter: () => localStorage.getItem('accessToken') || '' },
      }),
    ),
  ],
};
