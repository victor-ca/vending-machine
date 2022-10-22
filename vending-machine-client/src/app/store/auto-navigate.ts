import { SafeResourceUrl } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { CurrentUser } from '../auth/auth.service';

const getNextRoute = (user: CurrentUser): string => {
  if (user.isAuthenticated) {
    return user.isSeller ? 'seller' : 'store';
  }

  return 'auth';
};

export const autoNavigateAwayIfRequired = (
  user: CurrentUser,
  router: Router,
  currentUrl?: string
) => {
  {
    const nextPath = getNextRoute(user);
    const shouldBump = (currentUrl || '').indexOf(nextPath) !== 1;
    if (shouldBump) {
      router.navigate([nextPath]);
    }
  }
};
