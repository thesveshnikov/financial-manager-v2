import { Component, input, output } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';

import { RegisterInput } from '../register-input/register-input';
import { RegisterPassword } from '../register-password/register-password';
import { RegisterTerms } from '../register-terms/register-terms';

@Component({
  selector: 'app-register-form',
  standalone: true,
  imports: [ReactiveFormsModule, RegisterInput, RegisterPassword, RegisterTerms, RouterLink],
  templateUrl: './register-form.html',
  styleUrl: './register-form.scss',
})
export class RegisterForm {
  form = input.required<FormGroup>();
  submitted = input(false);
  submitSuccess = input(false);

  submittedForm = output<void>();

  showError(controlName: string): boolean {
    const control = this.form().get(controlName);

    return !!control && control.invalid && (control.touched || this.submitted());
  }

  getControlError(controlName: string): string {
    const control = this.form().get(controlName);

    if (control?.hasError('required')) {
      return controlName === 'userName'
        ? 'Username is required.'
        : controlName === 'email'
          ? 'Email is required.'
          : 'Date of birth is required.';
    }

    if (control?.hasError('minlength')) {
      return 'Username must have at least 3 characters.';
    }

    if (control?.hasError('email')) {
      return 'Please enter a valid email address.';
    }

    if (control?.hasError('minimumAge')) {
      return 'You must be at least 13 years old.';
    }

    return '';
  }
}
