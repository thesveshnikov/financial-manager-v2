import { Routes } from '@angular/router';

import { authGuard } from './core/auth/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'register',
    loadComponent: () => import('./core/auth/pages/register/register').then((c) => c.Register),
  },

  {
    path: 'login',
    loadComponent: () => import('./core/auth/pages/login/login').then((c) => c.Login),
  },

  {
    path: 'dashboard',
    loadComponent: () =>
      import('./features/dashboard/pages/dashboard/dashboard').then((c) => c.Dashboard),
    canMatch: [authGuard],
  },

  {
    path: 'expenses',
    loadComponent: () =>
      import('./features/expenses/pages/expenses/expenses').then((c) => c.Expenses),
    canMatch: [authGuard],
  },

  {
    path: 'categories',
    loadComponent: () =>
      import('./features/categories/pages/categories/categories').then((c) => c.Categories),
    canMatch: [authGuard],
  },

  {
    path: 'profile',
    loadComponent: () => import('./features/profile/pages/profile/profile').then((c) => c.Profile),
    canMatch: [authGuard],
  },

  {
    path: '**',
    redirectTo: 'dashboard',
  },
];
