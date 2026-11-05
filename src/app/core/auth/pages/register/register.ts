import { Component, inject } from '@angular/core';

import { FormControl, FormGroup, Validators } from '@angular/forms';

import { Router } from '@angular/router';

import { AuthService } from '../../services/auth.service';
import { RegisterUser } from '../../models/register-user.model';
import { RegisterForm } from '../../components/register-form/register-form';
import { minimumAgeValidator } from '../../validators/minimum-age.validator';
import { passwordMatchValidator } from '../../validators/password-match.validator';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [RegisterForm],
  templateUrl: './register.html',
  styleUrl: './register.scss',
})
export class Register {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  submitSuccess = false;
  registerFormSubmitted = false;

  registerForm = new FormGroup(
    {
      userName: new FormControl('', [Validators.required, Validators.minLength(3)]),

      dateOfBirthday: new FormControl('', [Validators.required, minimumAgeValidator(13)]),

      email: new FormControl('', [Validators.required, Validators.email]),

      password: new FormControl('', [
        Validators.required,
        Validators.minLength(8),
        Validators.pattern(
          /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*(),.?":{}|<>_\-+=\[\]\\/]).{8,}$/,
        ),
      ]),

      confirmPassword: new FormControl('', [Validators.required]),

      acceptTerms: new FormControl(false, [Validators.requiredTrue]),
    },
    {
      validators: passwordMatchValidator,
    },
  );

  onSubmit(): void {
    this.submitSuccess = false;
    this.registerFormSubmitted = true;

    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();

      return;
    }

    const formData = this.registerForm.getRawValue();

    const registrationData: RegisterUser = {
      name: formData.userName!,
      email: formData.email!,
      password: formData.password!,
      dateOfBirthday: `${formData.dateOfBirthday}T00:00:00.000Z`,
    };

    this.authService.register(registrationData).subscribe({
      next: () => {
        this.submitSuccess = true;

        this.router.navigate(['/login']);
      },

      error: (error) => {
        console.error('Registration error:', error);
      },
    });
  }
}
