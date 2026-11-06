import { Component, input, output } from '@angular/core';

@Component({
  selector: 'app-delete-category-modal',
  templateUrl: './delete-category-modal.html',
  styleUrl: './delete-category-modal.scss',
})
export class DeleteCategoryModal {
  categoryName = input.required<string>();

  confirm = output<void>();
  close = output<void>();

  isDeleting = false;

  confirmDelete(): void {
    if (this.isDeleting) return;

    this.confirm.emit();
  }

  closeModal(): void {
    if (this.isDeleting) return;

    this.close.emit();
  }
}
