import { Component, input, output } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { Category } from '../../models/category.model';
import { ExpenseFilters as ExpenseFiltersModel } from '../../models/expense-filters.model';

@Component({
  selector: 'app-expenses-filters',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './expenses-filters.html',
  styleUrl: './expenses-filters.scss',
})
export class ExpensesFilters {
  filters = input.required<ExpenseFiltersModel>();

  categories = input<Category[]>([]);

  filtersChange = output<ExpenseFiltersModel>();

  readonly Number = Number;

  readonly types = [
    { value: 0, label: 'Fixed' },
    { value: 1, label: 'Variable' },
  ];

  readonly paymentMethods = [
    { value: 0, label: 'Cash' },
    { value: 1, label: 'Card' },
    { value: 2, label: 'Pix' },
  ];

  update<K extends keyof ExpenseFiltersModel>(key: K, value: ExpenseFiltersModel[K]): void {
    this.filtersChange.emit({
      ...this.filters(),
      [key]: value,
    });
  }

  clear(): void {
    this.filtersChange.emit({
      search: '',
      type: '',
      paymentMethod: '',
      category: '',
      fromDate: '',
      toDate: '',
      minAmount: null,
      maxAmount: null,
    });
  }
}
