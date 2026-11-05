import { Component, input } from '@angular/core';
import { CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-expense-insights',
  imports: [CurrencyPipe],
  templateUrl: './expense-insights.html',
})
export class ExpenseInsights {
  mostExpensiveCategory = input('N/A');
  mostExpensiveCategoryTotal = input(0);

  averageExpense = input(0);
  highestExpense = input(0);
  lowestExpense = input(0);
}
