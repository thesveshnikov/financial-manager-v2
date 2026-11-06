import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

import { ProfileService } from '../../../../core/services/profile.service';

import { UserProfile } from '../../models/user-profile.model';

import { Sidebar } from '../../../../core/components/sidebar/sidebar';
import { ProfileForm } from '../../components/profile-form/profile-form';
import { AccountInformation } from '../../components/account-information/account-information';
import { DangerZone } from '../../components/danger-zone/danger-zone';
import { DeleteUserModal } from '../../components/delete-user-modal/delete-user-modal';

@Component({
  selector: 'app-profile',
  imports: [
    ReactiveFormsModule,
    Sidebar,
    ProfileForm,
    AccountInformation,
    DangerZone,
    DeleteUserModal,
  ],
  templateUrl: './profile.html',
  styleUrl: './profile.scss',
})
export class Profile implements OnInit {
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly fb = inject(FormBuilder);
  private readonly profileService = inject(ProfileService);

  profile: UserProfile | null = null;

  loading = true;
  editing = false;
  saving = false;
  deleting = false;

  isAddModalVisible = false;

  errorMessage = '';
  successMessage = '';

  form = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(100)]],
    email: [{ value: '', disabled: true }],
    dateOfBirthday: [{ value: '', disabled: true }],
  });

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile(): void {
    this.loading = true;
    this.errorMessage = '';

    this.profileService.profile().subscribe({
      next: (response) => {
        this.profile = response;

        this.form.patchValue({
          name: response.name,
          email: response.email ?? '',
          dateOfBirthday: response.dateOfBirthday ? response.dateOfBirthday.substring(0, 10) : '',
        });

        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.errorMessage = 'Unable to load your profile.';
        this.loading = false;
      },
    });
  }

  editProfile(): void {
    this.editing = true;
    this.successMessage = '';
    this.errorMessage = '';

    this.form.get('name')?.enable();
  }

  cancelEdit(): void {
    this.form.patchValue({
      name: this.profile?.name ?? '',
    });

    this.form.get('name')?.disable();

    this.editing = false;
    this.errorMessage = '';
  }

  saveChanges(): void {
    if (this.form.invalid || this.saving) {
      this.form.markAllAsTouched();
      return;
    }

    if (!this.profile?.email) {
      this.errorMessage = 'The API must return the user email before the profile can be updated.';
      return;
    }

    this.saving = true;
    this.errorMessage = '';
    this.successMessage = '';

    const name = this.form.getRawValue().name!;

    this.profileService
      .updateProfile({
        name,
        email: this.profile.email,
      })
      .subscribe({
        next: () => {
          this.profile = {
            ...this.profile!,
            name,
          };

          this.form.get('name')?.disable();

          this.editing = false;
          this.saving = false;

          this.successMessage = 'Profile updated successfully.';
        },
        error: () => {
          this.errorMessage = 'Unable to update your profile.';
          this.saving = false;
        },
      });
  }

  openDeleteModal(): void {
    this.isAddModalVisible = true;
  }

  closeDeleteModal(): void {
    this.isAddModalVisible = false;
  }

  deleteAccount(): void {
    if (this.deleting) return;

    this.deleting = true;
    this.errorMessage = '';

    this.profileService.deleteAccount().subscribe({
      next: () => {
        window.location.href = '/login';
      },
      error: () => {
        this.errorMessage = 'Unable to delete your account.';
        this.deleting = false;
        this.isAddModalVisible = false;
      },
    });
  }
}
