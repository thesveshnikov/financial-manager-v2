import { Component, computed, input } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-login-field',
  imports: [ReactiveFormsModule],
  templateUrl: './login-field.html',
  styleUrl: './login-field.scss',
})
export class LoginField {
  label = input.required<string>();
  type = input.required<string>();
  placeholder = input.required<string>();
  control = input.required<FormControl<string>>();
  submitted = input(false);

  showError = computed(() => {
    const control = this.control();

    return control.invalid && (control.touched || this.submitted());
  });

  errorMessage = computed(() => {
    const control = this.control();

    if (control.hasError('required')) {
      return `${this.label()} is required.`;
    }

    if (control.hasError('email')) {
      return 'Please enter a valid email address.';
    }

    return '';
  });
}
