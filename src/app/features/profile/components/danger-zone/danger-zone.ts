import { Component, input, output } from '@angular/core';

@Component({
  selector: 'app-danger-zone',
  templateUrl: './danger-zone.html',
  styleUrl: './danger-zone.scss',
})
export class DangerZone {
  deleting = input(false);

  delete = output<void>();
}
