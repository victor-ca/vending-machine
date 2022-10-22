import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { map, Observable, tap } from 'rxjs';
import { autoNavigateAwayIfRequired } from 'src/app/store/auto-navigate';
import { AuthService } from '../auth.service';

@Injectable({
  providedIn: 'root',
})
export class AuthBumperGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    return this.authService.currentUser$.pipe(
      tap((u) => autoNavigateAwayIfRequired(u, this.router, state.url)),
      map(() => true)
    );
  }
}
