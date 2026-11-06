import { Component, output, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-category-form',
  imports: [FormsModule],
  templateUrl: './category-form.html',
  styleUrl: './category-form.scss',
})
export class CategoryForm {
  add = output<string>();

  name = '';
  submitting = signal(false);

  submit(): void {
    const name = this.name.trim();

    if (!name || this.submitting()) return;

    this.add.emit(name);
  }

  setSubmitting(value: boolean): void {
    this.submitting.set(value);
  }

  reset(): void {
    this.name = '';
  }
}
