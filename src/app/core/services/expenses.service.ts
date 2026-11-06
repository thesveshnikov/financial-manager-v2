import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

import { Expense } from '../../features/expenses/models/expense.model';
import { ExpensePayload } from '../../features/expenses/models/expense-payload.model';

import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ExpensesService {
  private readonly http = inject(HttpClient);

  private base = environment.apiUrl;

  list() {
    return this.http.get<{ expenses: Expense[] }>(`${this.base}/api/expenses`);
  }

  create(data: ExpensePayload) {
    return this.http.post<Expense>(`${this.base}/api/expenses`, data);
  }

  update(id: string, data: ExpensePayload) {
    return this.http.put<void>(`${this.base}/api/expenses/${id}`, data);
  }

  delete(id: string) {
    return this.http.delete<void>(`${this.base}/api/expenses/${id}`);
  }
}
