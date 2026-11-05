import { Component, Input } from '@angular/core';
import { CurrencyPipe, DecimalPipe } from '@angular/common';
import { RouterLink } from '@angular/router';

import { CategorySummary as CategorySummaryModel } from '../../../../shared/models/category-summary.model';

@Component({
  selector: 'app-category-summary',
  imports: [CurrencyPipe, DecimalPipe, RouterLink],
  templateUrl: './category-summary.html',
})
export class CategorySummary {
  @Input() categories: CategorySummaryModel[] = [];
}
