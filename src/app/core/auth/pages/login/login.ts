import { Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

import { AuthService } from '../../services/auth.service';
import { Login as LoginModel } from '../../models/login.model';

import { LoginForm } from '../../components/login-form/login-form';

export type LoginFormGroup = FormGroup<{
  email: FormControl<string>;
  password: FormControl<string>;
}>;

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, LoginForm],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  readonly submitted = signal(false);
  readonly submitSuccess = signal(false);

  readonly loginForm: LoginFormGroup = new FormGroup({
    email: new FormControl('', {
      nonNullable: true,
      validators: [Validators.required, Validators.email],
    }),

    password: new FormControl('', {
      nonNullable: true,
      validators: [Validators.required],
    }),
  });

  onSubmit(): void {
    this.submitSuccess.set(false);
    this.submitted.set(true);

    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    const loginData: LoginModel = this.loginForm.getRawValue();

    this.authService.login(loginData).subscribe({
      next: () => {
        this.submitSuccess.set(true);
        this.router.navigate(['/dashboard']);
      },

      error: (error) => {
        console.error('Login error:', error);
      },
    });
  }
}
