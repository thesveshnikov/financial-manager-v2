import { Component, input } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { RouterLink } from '@angular/router';

import { Expense } from '../../../../shared/models/expense.model';

@Component({
  selector: 'app-recent-expenses',
  imports: [CurrencyPipe, RouterLink],
  templateUrl: './recent-expenses.html',
})
export class RecentExpenses {
  expenses = input<Expense[]>([]);
}
