import { Routes } from '@angular/router';
import { authGuard } from '../../Core/auth/auth.guard';

export const privateRoutes: Routes = [
  {
    path: 'perfil',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./pages/admin-dashboard/admin-dashboard').then((m) => m.AdminDashboard),
    title: 'PlayVerse | Mi perfil',
  },
  {
    path: 'admin',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./pages/admin-dashboard/admin-dashboard').then((m) => m.AdminDashboard),
    title: 'PlayVerse | Panel administrativo',
  },
  {
    path: 'dashboard',
    redirectTo: 'admin',
    pathMatch: 'full',
  },
];
