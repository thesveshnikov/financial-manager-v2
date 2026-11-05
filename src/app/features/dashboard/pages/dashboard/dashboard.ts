import { Component } from '@angular/core';

import { Sidebar } from '../../../../core/components/sidebar/sidebar';

@Component({
  imports: [Sidebar],
  selector: 'app-dashboard',
  styleUrl: './dashboard.scss',
  templateUrl: './dashboard.html',
})
export class Dashboard {}
