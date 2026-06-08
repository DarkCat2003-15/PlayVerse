import { Routes } from '@angular/router';
import { privateRoutes } from './Features/admin/admin.routes';
import { publicRoutes } from './Features/public/public.routes';

export const routes: Routes = [
  ...publicRoutes,
  ...privateRoutes,
  { path: 'products', redirectTo: 'juegos', pathMatch: 'full' },
  { path: 'about', redirectTo: 'quienes-somos', pathMatch: 'full' },
  { path: 'contact', redirectTo: 'contactanos', pathMatch: 'full' },
  { path: '**', redirectTo: '' },
];
