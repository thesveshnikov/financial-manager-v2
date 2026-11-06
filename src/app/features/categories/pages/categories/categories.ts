import { Component, inject, OnInit, signal, viewChild } from '@angular/core';

import { Sidebar } from '../../../../core/components/sidebar/sidebar';
import { CategoriesService } from '../../../../core/services/categories.service';

import { Category } from '../../../expenses/models/category.model';

import { CategoryForm } from '../../components/category-form/category-form';
import { CategoriesList } from '../../components/categories-list/categories-list';
import { EditCategoryModal } from '../../components/edit-category-modal/edit-category-modal';
import { DeleteCategoryModal } from '../../components/delete-category-modal/delete-category-modal';

@Component({
  selector: 'app-categories',
  imports: [Sidebar, CategoryForm, CategoriesList, EditCategoryModal, DeleteCategoryModal],
  templateUrl: './categories.html',
  styleUrl: './categories.scss',
})
export class Categories implements OnInit {
  private readonly categoriesService = inject(CategoriesService);

  private readonly categoryForm = viewChild(CategoryForm);

  categories = signal<Category[]>([]);
  loading = signal(false);
  submitting = signal(false);
  errorMessage = signal('');

  selectedCategory: Category | null = null;

  isDeleteModalVisible = false;
  isEditModalVisible = false;

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories(): void {
    this.loading.set(true);
    this.errorMessage.set('');

    this.categoriesService.list().subscribe({
      next: (response) => {
        this.categories.set(response.categories ?? []);
        this.loading.set(false);
      },
      error: () => {
        this.errorMessage.set('The categories could not be loaded.');
        this.loading.set(false);
      },
    });
  }

  addCategory(name: string): void {
    if (this.submitting()) return;

    this.submitting.set(true);
    this.errorMessage.set('');

    this.categoryForm()?.setSubmitting(true);

    this.categoriesService.create(name).subscribe({
      next: (category) => {
        this.categories.update((categories) => [...categories, category]);

        this.categoryForm()?.reset();

        this.submitting.set(false);
        this.categoryForm()?.setSubmitting(false);
      },
      error: () => {
        this.errorMessage.set('The category could not be registered.');

        this.submitting.set(false);
        this.categoryForm()?.setSubmitting(false);
      },
    });
  }

  editCategory(category: Category): void {
    this.selectedCategory = category;
    this.isEditModalVisible = true;
  }

  deleteCategory(category: Category): void {
    this.selectedCategory = category;
    this.isDeleteModalVisible = true;
  }

  confirmDeleteCategory(): void {
    if (!this.selectedCategory) return;

    this.categoriesService.delete(this.selectedCategory.id).subscribe({
      next: () => {
        this.isDeleteModalVisible = false;
        this.selectedCategory = null;

        this.loadCategories();
      },
      error: () => {
        this.errorMessage.set('Unable to delete category.');

        this.isDeleteModalVisible = false;
        this.selectedCategory = null;
      },
    });
  }

  closeDeleteModal(): void {
    this.isDeleteModalVisible = false;
    this.selectedCategory = null;
  }

  closeEditModal(): void {
    this.isEditModalVisible = false;
    this.selectedCategory = null;

    this.loadCategories();
  }
}
