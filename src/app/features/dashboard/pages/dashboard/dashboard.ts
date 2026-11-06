import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';

import { ExpensesService } from '../../../../core/services/expenses.service';
import { CategoriesService } from '../../../../core/services/categories.service';

import { Sidebar } from '../../../../core/components/sidebar/sidebar';

import { Expense } from '../../../expenses/models/expense.model';
import { Category } from '../../../expenses/models/category.model';
import { CategorySummary } from '../../models/category-summary.model';

import { DashboardStats } from '../../components/dashboard-stats/dashboard-stats';
import { RecentExpenses } from '../../components/recent-expenses/recent-expenses';
import { CategorySummary as CategorySummaryComponent } from '../../components/category-summary/category-summary';
import { ExpenseInsights } from '../../components/expense-insights/expense-insights';
import { DashboardActions } from '../../components/dashboard-actions/dashboard-actions';

@Component({
  imports: [
    Sidebar,
    DashboardStats,
    RecentExpenses,
    CategorySummaryComponent,
    ExpenseInsights,
    DashboardActions,
    RouterLink,
  ],
  selector: 'app-dashboard',
  styleUrl: './dashboard.scss',
  templateUrl: './dashboard.html',
})
export class Dashboard implements OnInit {
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly expensesService = inject(ExpensesService);
  private readonly categoriesService = inject(CategoriesService);

  expenses: Expense[] = [];
  categories: Category[] = [];

  totalExpenses = 0;
  fixedExpenses = 0;
  variableExpenses = 0;
  monthlyExpenses = 0;

  totalTransactions = 0;
  fixedCount = 0;
  variableCount = 0;
  monthlyCount = 0;

  mostExpensiveCategory = 'N/A';
  mostExpensiveCategoryTotal = 0;

  averageExpense = 0;
  highestExpense = 0;
  lowestExpense = 0;

  recentExpenses: Expense[] = [];
  categorySummaries: CategorySummary[] = [];

  ngOnInit(): void {
    this.loadExpenses();
    this.loadCategories();
  }

  private loadExpenses(): void {
    this.expensesService.list().subscribe({
      next: (res) => {
        this.expenses = res.expenses ?? [];
        this.calculateDashboard();
        this.cdr.detectChanges();
      },
      error: (error) => console.error('Error loading expenses:', error),
    });
  }

  private loadCategories(): void {
    this.categoriesService.list().subscribe({
      next: (res) => {
        this.categories = res.categories ?? [];

        this.calculateDashboard();
        this.cdr.detectChanges();
      },
      error: (error) => console.error('Error loading categories:', error),
    });
  }

  private calculateDashboard(): void {
    const expenses = this.expenses;

    this.totalTransactions = expenses.length;

    this.totalExpenses = expenses.reduce(
      (total, expense) => total + Number(expense.amount || 0),
      0,
    );

    const fixed = expenses.filter((expense) => expense.type === 0);
    const variable = expenses.filter((expense) => expense.type === 1);

    this.fixedCount = fixed.length;
    this.variableCount = variable.length;

    this.fixedExpenses = fixed.reduce((total, expense) => total + Number(expense.amount || 0), 0);

    this.variableExpenses = variable.reduce(
      (total, expense) => total + Number(expense.amount || 0),
      0,
    );

    const now = new Date();

    const monthly = expenses.filter((expense) => {
      const date = new Date(expense.expenseDate);

      return date.getMonth() === now.getMonth() && date.getFullYear() === now.getFullYear();
    });

    this.monthlyCount = monthly.length;

    this.monthlyExpenses = monthly.reduce(
      (total, expense) => total + Number(expense.amount || 0),
      0,
    );

    this.averageExpense = expenses.length ? this.totalExpenses / expenses.length : 0;

    const amounts = expenses.map((expense) => Number(expense.amount || 0));

    this.highestExpense = amounts.length ? Math.max(...amounts) : 0;
    this.lowestExpense = amounts.length ? Math.min(...amounts) : 0;

    this.recentExpenses = [...expenses]
      .sort((a, b) => new Date(b.expenseDate).getTime() - new Date(a.expenseDate).getTime())
      .slice(0, 5);

    this.calculateCategorySummary();
  }

  private calculateCategorySummary(): void {
    const summary = new Map<string, CategorySummary>();

    this.expenses.forEach((expense) => {
      const categoryId = expense.categoryId;

      if (!categoryId) return;

      const category = this.categories.find((item) => item.id === categoryId);

      const current = summary.get(categoryId) ?? {
        id: categoryId,
        name: category?.name ?? 'Uncategorized',
        total: 0,
        percentage: 0,
        quantity: 0,
      };

      current.total += Number(expense.amount || 0);
      current.quantity++;

      summary.set(categoryId, current);
    });

    this.categorySummaries = Array.from(summary.values())
      .map((category) => ({
        ...category,
        percentage: this.totalExpenses > 0 ? (category.total / this.totalExpenses) * 100 : 0,
      }))
      .sort((a, b) => b.total - a.total);

    const highest = this.categorySummaries[0];

    this.mostExpensiveCategory = highest?.name ?? 'N/A';
    this.mostExpensiveCategoryTotal = highest?.total ?? 0;
  }
}
