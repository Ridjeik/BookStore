import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';
import { MessageService } from 'primeng/api';

export const adminGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const toastService = inject(MessageService)

  if (authService.isLoggedIn()) {
    if (authService.getUserInfo().role === 'admin') {
      return true;
    } else {
      toastService.add({severity:'error', summary:'Error', detail:'You need to be an admin to access this page'});
      return false
    }
    
  } else {
    alert('You need to login to access this page');
    return false;
  }
};
