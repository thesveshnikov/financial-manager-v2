import { Component, input } from '@angular/core';

@Component({
  selector: 'app-account-information',
  templateUrl: './account-information.html',
  styleUrl: './account-information.scss',
})
export class AccountInformation {
  memberSince = input<string | null>(null);
  userId = input<string | null>(null);
  status = input('Active');
}
