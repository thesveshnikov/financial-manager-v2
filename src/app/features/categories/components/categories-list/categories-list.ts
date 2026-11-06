import { Component, input, output } from '@angular/core';

import { Category } from '../../../expenses/models/category.model';

@Component({
  selector: 'app-categories-list',
  templateUrl: './categories-list.html',
  styleUrl: './categories-list.scss',
})
export class CategoriesList {
  categories = input.required<Category[]>();
  loading = input(false);

  edit = output<Category>();
  delete = output<Category>();
}
