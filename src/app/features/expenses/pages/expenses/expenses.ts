import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

import { Sidebar } from '../../../../core/components/sidebar/sidebar';

import { ExpensesService } from '../../../../core/services/expenses.service';
import { CategoriesService } from '../../../../core/services/categories.service';

import { Expense } from '../../models/expense.model';
import { Category } from '../../../../shared/models/category.model';
import { ExpenseFilters as ExpenseFiltersModel } from '../../models/expense-filters.model';

import { ExpensesFilters } from '../../components/expenses-filters/expenses-filters';

import { ExpensesList } from '../../components/expenses-list/expenses-list';
import { ExpenseFormModal } from '../../components/expense-form-modal/expense-form-modal';
import { DeleteExpenseModal } from '../../components/delete-expense-modal/delete-expense-modal';

@Component({
  selector: 'app-expenses',
  imports: [
    CommonModule,
    Sidebar,
    ExpensesFilters,
    ExpensesList,
    ExpenseFormModal,
    DeleteExpenseModal,
  ],
  templateUrl: './expenses.html',
  styleUrl: './expenses.scss',
})
export class Expenses implements OnInit {
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly expensesService = inject(ExpensesService);
  private readonly categoriesService = inject(CategoriesService);

  expenses: Expense[] = [];
  categories: Category[] = [];

  selectedExpense: Expense | null = null;

  isLoading = false;
  errorMessage = '';

  isFormModalVisible = false;
  isDeleteModalVisible = false;

  isDeleting = false;

  filters: ExpenseFiltersModel = {
    search: '',
    type: '',
    paymentMethod: '',
    category: '',
    fromDate: '',
    toDate: '',
    minAmount: null,
    maxAmount: null,
  };

  ngOnInit(): void {
    this.loadCategories();
    this.loadExpenses();
  }

  private loadCategories(): void {
    this.categoriesService.list().subscribe({
      next: (response) => {
        this.categories = response?.categories ?? [];

        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Error loading categories:', error);
      },
    });
  }

  private loadExpenses(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.expensesService.list().subscribe({
      next: (response) => {
        this.expenses = response.expenses ?? [];
        this.isLoading = false;

        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Error loading expenses:', error);

        this.errorMessage = 'Unable to load expenses.';
        this.isLoading = false;
      },
    });
  }

  get filteredExpenses(): Expense[] {
    return this.expenses.filter((expense) => {
      const f = this.filters;

      const expenseDate = new Date(expense.expenseDate).toISOString().split('T')[0];

      return (
        expense.name.toLowerCase().includes(f.search.toLowerCase()) &&
        (f.type === '' || expense.type === Number(f.type)) &&
        (f.paymentMethod === '' || expense.paymentMethod === Number(f.paymentMethod)) &&
        (f.category === '' || expense.categoryId === f.category) &&
        (!f.fromDate || expenseDate >= f.fromDate) &&
        (!f.toDate || expenseDate <= f.toDate) &&
        (f.minAmount === null || expense.amount >= f.minAmount) &&
        (f.maxAmount === null || expense.amount <= f.maxAmount)
      );
    });
  }

  get totalAmount(): number {
    return this.filteredExpenses.reduce((total, expense) => total + expense.amount, 0);
  }

  openAddModal(): void {
    this.selectedExpense = null;
    this.isFormModalVisible = true;
  }

  openEditModal(expense: Expense): void {
    this.selectedExpense = expense;
    this.isFormModalVisible = true;
  }

  closeFormModal(): void {
    this.isFormModalVisible = false;
    this.selectedExpense = null;

    this.loadExpenses();
  }

  openDeleteModal(expense: Expense): void {
    this.selectedExpense = expense;
    this.isDeleteModalVisible = true;
  }

  closeDeleteModal(): void {
    if (this.isDeleting) return;

    this.isDeleteModalVisible = false;
    this.selectedExpense = null;
  }

  confirmDelete(): void {
    if (!this.selectedExpense || this.isDeleting) return;

    this.isDeleting = true;

    this.expensesService.delete(this.selectedExpense.id).subscribe({
      next: () => {
        this.isDeleting = false;
        this.isDeleteModalVisible = false;
        this.selectedExpense = null;

        this.loadExpenses();
      },
      error: (error) => {
        console.error('Error deleting expense:', error);

        this.errorMessage = 'Unable to delete expense.';

        this.isDeleting = false;
        this.isDeleteModalVisible = false;
        this.selectedExpense = null;
      },
    });
  }
}
