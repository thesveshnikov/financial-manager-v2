import { Component, forwardRef, input } from '@angular/core';

import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-register-input',
  standalone: true,
  templateUrl: './register-input.html',
  styleUrl: './register-input.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => RegisterInput),
      multi: true,
    },
  ],
})
export class RegisterInput implements ControlValueAccessor {
  label = input.required<string>();
  placeholder = input<string>();
  type = input<string>('text');
  errorMessage = input<string>('');
  invalid = input<boolean>(false);

  value = '';
  disabled = false;

  private onChange: (value: string) => void = () => {};
  private onTouched: () => void = () => {};

  writeValue(value: string | null): void {
    this.value = value ?? '';
  }

  registerOnChange(fn: (value: string) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(disabled: boolean): void {
    this.disabled = disabled;
  }

  updateValue(value: string): void {
    this.value = value;
    this.onChange(value);
  }

  markAsTouched(): void {
    this.onTouched();
  }
}
