import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
//guard omogucava samo da ne moze da se navigate ko nisu uslovi ispunjeni 
export const authGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);

  const isLoggedIn = !!localStorage.getItem('token');

  if (!isLoggedIn) {
    router.navigate(['/']);
    return false;
  }

  return true;
};