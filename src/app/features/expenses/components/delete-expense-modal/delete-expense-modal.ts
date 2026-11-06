import { Component, input, output } from '@angular/core';

@Component({
  selector: 'app-delete-expense-modal',
  standalone: true,
  templateUrl: './delete-expense-modal.html',
  styleUrl: './delete-expense-modal.scss',
})
export class DeleteExpenseModal {
  expenseName = input.required<string>();
  isDeleting = input(false);

  confirm = output<void>();
  close = output<void>();

  confirmDelete(): void {
    if (this.isDeleting()) return;

    this.confirm.emit();
  }

  closeModal(): void {
    if (this.isDeleting()) return;

    this.close.emit();
  }
}
