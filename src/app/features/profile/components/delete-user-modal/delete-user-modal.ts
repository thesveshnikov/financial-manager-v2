import { Component, output, signal } from '@angular/core';

@Component({
  selector: 'app-delete-user-modal',
  templateUrl: './delete-user-modal.html',
  styleUrl: './delete-user-modal.scss',
})
export class DeleteUserModal {
  confirm = output<void>();
  close = output<void>();

  confirmation = signal('');

  isDeleting = signal(false);

  get canDelete(): boolean {
    return this.confirmation() === 'DELETE' && !this.isDeleting();
  }

  confirmDelete(): void {
    if (!this.canDelete) return;

    this.isDeleting.set(true);
    this.confirm.emit();
  }

  closeModal(): void {
    if (this.isDeleting()) return;

    this.close.emit();
  }
}
