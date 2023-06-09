import { CanActivateFn } from '@angular/router';

export const authorguardGuard: CanActivateFn = (route, state) => {
  return true;
};
