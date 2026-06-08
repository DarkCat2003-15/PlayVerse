import { Routes } from '@angular/router';

export const publicRoutes: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/home/home').then((m) => m.Home),
    title: 'PlayVerse | Inicio',
  },
  {
    path: 'juegos',
    loadComponent: () => import('./pages/products/products').then((m) => m.Products),
    title: 'PlayVerse | Juegos',
  },
  {
    path: 'quienes-somos',
    loadComponent: () => import('./pages/about/about').then((m) => m.About),
    title: 'PlayVerse | Quienes somos',
  },
  {
    path: 'contactanos',
    loadComponent: () => import('./pages/contact/contact').then((m) => m.Contact),
    title: 'PlayVerse | Contactanos',
  },
  {
    path: 'login',
    loadComponent: () => import('../auth/pages/login/login').then((m) => m.Login),
    title: 'PlayVerse | Login',
  },
];
