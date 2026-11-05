import { Component, input } from '@angular/core';
import { CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-dashboard-stats',
  imports: [CurrencyPipe],
  templateUrl: './dashboard-stats.html',
  styleUrl: './dashboard-stats.scss',
})
export class DashboardStats {
  totalExpenses = input(0);
  totalTransactions = input(0);

  fixedExpenses = input(0);
  fixedCount = input(0);

  variableExpenses = input(0);
  variableCount = input(0);

  monthlyExpenses = input(0);
  monthlyCount = input(0);
}
