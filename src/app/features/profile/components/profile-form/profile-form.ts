import { Component, input, output } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-profile-form',
  imports: [ReactiveFormsModule],
  templateUrl: './profile-form.html',
})
export class ProfileForm {
  form = input.required<FormGroup>();

  editing = input(false);
  saving = input(false);

  errorMessage = input('');
  successMessage = input('');

  edit = output<void>();
  cancel = output<void>();
  save = output<void>();
}
