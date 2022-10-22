import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { distinctUntilChanged, filter, map, noop, switchMap } from 'rxjs';
import { AuthenticatedUser, AuthService } from './auth/auth.service';
import { autoNavigateAwayIfRequired } from './store/auto-navigate';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  constructor(private authService: AuthService, private router: Router) {}
  ngOnInit(): void {
    this.authService.init().subscribe((user) => {
      autoNavigateAwayIfRequired(user, this.router, this.router.url);
    });

    this.authService.currentUser$
      .pipe(
        filter((x) => x.isAuthenticated),
        map((u) => (u as AuthenticatedUser).name),
        distinctUntilChanged(),
        switchMap(() => this.authService.getActiveSessionsCount()),
        filter((sessionCount) => sessionCount > 1)
      )
      .subscribe((sessionCount) => {
        var msg =
          `you have ${sessionCount - 1} more sessions\n` +
          `Do you want to drop them?`;
        if (confirm(msg)) {
          this.authService.dropAllOtherSessions().subscribe(noop);
        }
      });
  }

  handleActiveSessions() {
    throw new Error('Method not implemented.');
  }
}
