import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { AuthService, CurrentUser } from '../auth.service';
import { logoutActions } from '../store/auth.actions';

@Component({
  selector: 'app-logged-in-user',
  templateUrl: './logged-in-user.component.html',
  styleUrls: ['./logged-in-user.component.scss'],
})
export class LoggedInUserComponent implements OnInit {
  constructor(private authService: AuthService, private store: Store) {}
  authState$!: Observable<CurrentUser>;
  ngOnInit(): void {
    this.authState$ = this.authService.currentUser$;
  }
  logout() {
    this.store.dispatch(logoutActions.start());
  }
}
