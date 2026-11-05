import { Component, input, output } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import { LoginFormGroup } from '../../pages/login/login';

import { LoginField } from '../login-field/login-field';
import { LoginFooter } from '../login-footer/login-footer';

@Component({
  selector: 'app-login-form',
  imports: [ReactiveFormsModule, LoginField, LoginFooter],
  templateUrl: './login-form.html',
  styleUrl: './login-form.scss',
})
export class LoginForm {
  form = input.required<LoginFormGroup>();

  submitted = input(false);

  submittedForm = output<void>();
}
