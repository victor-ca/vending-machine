import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { FormGroupState } from 'ngrx-forms';
import { Observable, take } from 'rxjs';
import { loginUserActions } from '../../../auth/store/auth.actions';
import { selectLoginForm } from '../store/auth-ui.selectors';
import { LoginForm } from '../store/auth-ui.state';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  formState$!: Observable<FormGroupState<LoginForm>>;
  constructor(private readonly store: Store) {
    this.formState$ = this.store.select(selectLoginForm);
  }

  ngOnInit(): void {}
  login() {
    this.formState$.pipe(take(1)).subscribe(({ controls }) => {
      const { password, userName } = controls;
      this.store.dispatch(
        loginUserActions.start({
          password: password.value,
          userName: userName.value,
        })
      );
    });
  }
}
