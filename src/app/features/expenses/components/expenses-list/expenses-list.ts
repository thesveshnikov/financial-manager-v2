import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';

import { Expense } from '../../models/expense.model';
import { Category } from '../../models/category.model';

@Component({
  selector: 'app-expenses-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './expenses-list.html',
  styleUrl: './expenses-list.scss',
})
export class ExpensesList {
  expenses = input<Expense[]>([]);
  categories = input<Category[]>([]);

  totalExpenses = input(0);
  totalAmount = input(0);

  isLoading = input(false);
  errorMessage = input('');

  edit = output<Expense>();
  delete = output<Expense>();

  readonly paymentMethods = ['Cash', 'Card', 'Pix'];
  readonly types = ['Fixed', 'Variable'];

  getPaymentMethod(value: number): string {
    return this.paymentMethods[value] ?? 'Unknown';
  }

  getType(value: number): string {
    return this.types[value] ?? 'Unknown';
  }

  getCategoryName(expense: Expense): string {
    return (
      expense.categoryName ??
      this.categories().find((category) => category.id === expense.categoryId)?.name ??
      'Uncategorized'
    );
  }
}
