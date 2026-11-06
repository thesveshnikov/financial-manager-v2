import { Component, ChangeDetectorRef, inject, input, output, OnInit } from '@angular/core';

import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

import { ExpensesService } from '../../../../core/services/expenses.service';
import { CategoriesService } from '../../../../core/services/categories.service';

import { Expense } from '../../models/expense.model';
import { Category } from '../../models/category.model';
import { ExpensePayload } from '../../models/expense-payload.model';

@Component({
  selector: 'app-expense-form-modal',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './expense-form-modal.html',
  styleUrl: './expense-form-modal.scss',
})
export class ExpenseFormModal implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly cdr = inject(ChangeDetectorRef);

  private readonly expensesService = inject(ExpensesService);
  private readonly categoriesService = inject(CategoriesService);

  expense = input<Expense | null>(null);

  close = output<void>();

  categories: Category[] = [];

  isSubmitting = false;
  isLoadingCategories = false;

  errorMessage = '';

  get isEditing(): boolean {
    return !!this.expense();
  }

  readonly paymentMethods = {
    cash: 0,
    card: 1,
    pix: 2,
  };

  readonly expenseTypes = {
    fixed: 0,
    variable: 1,
  };

  readonly form = this.fb.nonNullable.group({
    date: [this.getToday(), Validators.required],
    name: ['', [Validators.required, Validators.maxLength(100)]],
    amount: [0, [Validators.required, Validators.min(0.01)]],
    paymentMethod: ['cash', Validators.required],
    categoryId: ['', Validators.required],
    type: ['variable', Validators.required],
  });

  ngOnInit(): void {
    this.loadCategories();

    if (this.expense()) {
      this.populateForm();
    }
  }

  private getToday(): string {
    const today = new Date();

    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');

    return `${year}-${month}-${day}`;
  }

  private populateForm(): void {
    const expense = this.expense()!;

    this.form.patchValue({
      date: this.formatDate(expense.expenseDate),
      name: expense.name,
      amount: expense.amount,
      paymentMethod: this.getPaymentMethodKey(expense.paymentMethod),
      categoryId: expense.categoryId ?? '',
      type: expense.type === 0 ? 'fixed' : 'variable',
    });
  }

  private formatDate(date: string): string {
    const parsedDate = new Date(date);

    const year = parsedDate.getFullYear();
    const month = String(parsedDate.getMonth() + 1).padStart(2, '0');
    const day = String(parsedDate.getDate()).padStart(2, '0');

    return `${year}-${month}-${day}`;
  }

  private getPaymentMethodKey(value: number): string {
    return (
      Object.entries(this.paymentMethods).find(([, methodValue]) => methodValue === value)?.[0] ??
      'cash'
    );
  }

  private loadCategories(): void {
    this.isLoadingCategories = true;

    this.categoriesService.list().subscribe({
      next: (response) => {
        this.categories = response?.categories ?? [];
        this.isLoadingCategories = false;

        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Error loading categories:', error);

        this.errorMessage = 'The categories could not be loaded.';
        this.isLoadingCategories = false;

        this.cdr.detectChanges();
      },
    });
  }

  submit(): void {
    this.errorMessage = '';

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const value = this.form.getRawValue();

    const payload: ExpensePayload = {
      name: value.name.trim(),
      amount: Number(value.amount),

      paymentMethod: this.paymentMethods[value.paymentMethod as keyof typeof this.paymentMethods],

      type: this.expenseTypes[value.type as keyof typeof this.expenseTypes],

      expenseDate: new Date(`${value.date}T12:00:00`).toISOString(),

      categoryId: value.categoryId,
    };

    this.isSubmitting = true;

    const handleSuccess = (): void => {
      this.isSubmitting = false;
      this.close.emit();

      this.cdr.detectChanges();
    };

    const handleError = (error: unknown): void => {
      const response = error as {
        error?: {
          errors?: string[];
        };
      };

      this.errorMessage =
        response?.error?.errors?.join(', ') ??
        `Unable to ${this.isEditing ? 'update' : 'create'} expense.`;

      this.isSubmitting = false;

      this.cdr.detectChanges();
    };

    if (this.expense()) {
      this.expensesService.update(this.expense()!.id, payload).subscribe({
        next: handleSuccess,
        error: handleError,
      });
    } else {
      this.expensesService.create(payload).subscribe({
        next: handleSuccess,
        error: handleError,
      });
    }
  }

  closeModal(): void {
    if (this.isSubmitting) return;

    this.close.emit();
  }
}
