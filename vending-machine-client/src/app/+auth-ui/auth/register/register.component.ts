import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { FormGroupState } from 'ngrx-forms';
import { Observable, take } from 'rxjs';
import { registerActions } from '../../../auth/store/auth.actions';
import { selectRegisterForm } from '../store/auth-ui.selectors';
import { RegisterForm } from '../store/auth-ui.state';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {
  formState$!: Observable<FormGroupState<RegisterForm>>;
  constructor(private readonly store: Store) {
    this.formState$ = this.store.select(selectRegisterForm);
  }

  ngOnInit(): void {}
  register() {
    this.formState$.pipe(take(1)).subscribe(({ controls }) => {
      const { password, userName, isSeller } = controls;
      this.store.dispatch(
        registerActions.start({
          password: password.value,
          userName: userName.value,
          isSeller: isSeller.value,
        })
      );
    });
  }
}
