import { Component, input } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-register-terms',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './register-terms.html',
  styleUrl: './register-terms.scss',
})
export class RegisterTerms {
  control = input.required<FormControl<boolean | null>>();
  submitted = input(false);

  get showError(): boolean {
    return this.control().invalid && (this.control().touched || this.submitted());
  }
}
