import { Component, inject, input, output } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

import { CategoriesService } from '../../../../core/services/categories.service';

import { Category } from '../../../../shared/models/category.model';

@Component({
  selector: 'app-edit-category-modal',
  imports: [ReactiveFormsModule],
  templateUrl: './edit-category-modal.html',
  styleUrl: './edit-category-modal.scss',
})
export class EditCategoryModal {
  private readonly fb = inject(FormBuilder);
  private readonly categoriesService = inject(CategoriesService);

  category = input.required<Category>();
  close = output<void>();

  isSubmitting = false;
  errorMessage = '';

  readonly form = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(100)]],
  });

  ngOnInit(): void {
    this.form.patchValue({
      name: this.category().name,
    });
  }

  submit(): void {
    this.errorMessage = '';

    if (this.form.invalid || this.isSubmitting) {
      this.form.markAllAsTouched();
      return;
    }

    const { name } = this.form.getRawValue();

    this.isSubmitting = true;

    this.categoriesService.update(this.category().id, name.trim()).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.close.emit();
      },
      error: (error) => {
        this.errorMessage =
          error?.error?.errors?.join(', ') ??
          'We were unable to update the category. Please try again.';

        this.isSubmitting = false;
      },
    });
  }

  closeModal(): void {
    if (this.isSubmitting) return;

    this.close.emit();
  }
}
