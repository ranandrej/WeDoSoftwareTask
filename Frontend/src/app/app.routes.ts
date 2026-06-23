import { Routes } from '@angular/router';
import { authGuard } from './guards/auth-guard';
export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    loadComponent: () => import('./pages/home/home').then((m) => m.Home),

  },
  {
    path: 'workouts',
    loadComponent: () => import('./pages/workouts/workouts').then((m) => m.Workouts),
    canActivate: [authGuard],
  },
  
];
