import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Category } from '../../features/expenses/models/category.model';

import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class CategoriesService {
  private readonly http = inject(HttpClient);

  private base = environment.apiUrl;

  list() {
    return this.http.get<{ categories: Category[] }>(`${this.base}/api/categories`);
  }

  create(name: string) {
    return this.http.post<Category>(`${this.base}/api/categories`, { name });
  }

  update(id: string, name: string) {
    return this.http.put<void>(`${this.base}/api/categories/${id}`, { name });
  }

  delete(id: string) {
    return this.http.delete<void>(`${this.base}/api/categories/${id}`);
  }
}
