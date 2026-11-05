import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'register',
    loadComponent: () => import('./core/auth/pages/register/register').then((c) => c.Register),
  },
];
