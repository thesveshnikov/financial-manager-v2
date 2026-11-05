import { Component, input, Input } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';

import { RegisterInput } from '../register-input/register-input';

@Component({
  selector: 'app-register-password',
  standalone: true,
  imports: [ReactiveFormsModule, RegisterInput],
  templateUrl: './register-password.html',
  styleUrl: './register-password.scss',
})
export class RegisterPassword {
  form = input.required<FormGroup>();
  submitted = input(false);

  showError(controlName: string): boolean {
    const control = this.form().get(controlName);

    return !!control && control.invalid && (control.touched || this.submitted());
  }

  getPasswordError(): string {
    const control = this.form().get('password');

    if (control?.hasError('required')) {
      return 'Password is required.';
    }

    if (control?.hasError('minlength')) {
      return 'Password must have at least 8 characters.';
    }

    if (control?.hasError('pattern')) {
      return 'Password must contain uppercase, lowercase, number and special character.';
    }

    return '';
  }

  getConfirmPasswordError(): string {
    const control = this.form().get('confirmPassword');

    if (control?.hasError('required')) {
      return 'Please confirm your password.';
    }

    if (this.form().hasError('passwordMismatch')) {
      return 'Passwords do not match.';
    }

    return '';
  }

  generateStrongPassword(): void {
    const uppercase = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
    const lowercase = 'abcdefghijklmnopqrstuvwxyz';
    const numbers = '0123456789';
    const special = '!@#$%^&*()_+-=';

    const allCharacters = uppercase + lowercase + numbers + special;

    const getRandom = (characters: string): string => {
      const random = new Uint32Array(1);
      crypto.getRandomValues(random);

      return characters[random[0] % characters.length];
    };

    const password = [
      getRandom(uppercase),
      getRandom(lowercase),
      getRandom(numbers),
      getRandom(special),
      ...Array.from({ length: 8 }, () => getRandom(allCharacters)),
    ];

    for (let i = password.length - 1; i > 0; i--) {
      const random = new Uint32Array(1);
      crypto.getRandomValues(random);

      const j = random[0] % (i + 1);

      [password[i], password[j]] = [password[j], password[i]];
    }

    const generatedPassword = password.join('');

    this.form().patchValue({
      password: generatedPassword,
      confirmPassword: generatedPassword,
    });
  }
}
