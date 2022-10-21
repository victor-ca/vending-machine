import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './auth/auth.service';
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
      console.warn({ user });
      autoNavigateAwayIfRequired(user, this.router, this.router.url);
    });
  }
}
