import { Component, inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { finalize } from 'rxjs';
import { AuthService } from '../../services/auth';
import { User, UpdateUserDto } from '../../models/user.type';
import { ConfirmDeleteModal } from '../../components/confirm-delete-modal/confirm-delete-modal';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [FormsModule, ConfirmDeleteModal],
  templateUrl: './profile.html',
})
export class Profile implements OnInit {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  private originalUser: User | null = null;

  name = '';
  surename = '';
  email = '';
  password = '';
  confirmPassword = '';

  pageLoading = signal(true);
  loadError = signal('');
  editMode = signal(false);
  changePassword = signal(false);
  saving = signal(false);
  deleting = signal(false);
  showDeleteModal = signal(false);
  successMessage = signal('');
  errorMessage = signal('');

  ngOnInit(): void {
    this.loadProfile();
  }

  startEdit(): void {
    this.editMode.set(true);
    this.successMessage.set('');
    this.errorMessage.set('');
  }

  cancelEdit(): void {
    this.resetFormFromOriginal();
    this.editMode.set(false);
    this.changePassword.set(false);
    this.errorMessage.set('');
  }

  togglePasswordEdit(): void {
    this.changePassword.update((value) => !value);
    this.password = '';
    this.confirmPassword = '';
  }

  saveProfile(): void {
    if (!this.originalUser) {
      return;
    }

    const payload = this.buildUpdatePayload();
    if (Object.keys(payload).length === 0) {
      this.errorMessage.set('No changes to save.');
      return;
    }

    if (this.changePassword() && this.password !== this.confirmPassword) {
      this.errorMessage.set('Passwords do not match.');
      return;
    }

    this.saving.set(true);
    this.successMessage.set('');
    this.errorMessage.set('');

    this.authService
      .updateUser(this.originalUser.id, payload)
      .pipe(finalize(() => this.saving.set(false)))
      .subscribe({
        next: (user) => {
          this.applyUser(user);
          this.editMode.set(false);
          this.changePassword.set(false);
          this.password = '';
          this.confirmPassword = '';
          this.successMessage.set('Profile updated successfully.');
        },
        error: (err: HttpErrorResponse) => {
         this.errorMessage.set(err?.error || 'Updating user failed. Please try again.');
         console.error('Updating error:', err);
        },
      });
  }

  openDeleteModal(): void {
    this.showDeleteModal.set(true);
  }

  closeDeleteModal(): void {
    this.showDeleteModal.set(false);
  }

  onDeleteConfirmed(): void {
    if (!this.originalUser) {
      return;
    }

    this.showDeleteModal.set(false);
    this.deleting.set(true);
    this.errorMessage.set('');

    this.authService
      .deleteUser(this.originalUser.id)
      .pipe(finalize(() => this.deleting.set(false)))
      .subscribe({
        next: () => {
          this.router.navigate(['/']);
        },
        error: (err: HttpErrorResponse) => {
          this.errorMessage.set(err?.error || 'Deleting user failed. Please try again.');
          console.error('Deletig error:', err);
        },
      });
  }

  private loadProfile(): void {
    this.pageLoading.set(true);
    this.loadError.set('');

    this.authService
      .getMe()
      .pipe(finalize(() => this.pageLoading.set(false)))
      .subscribe({
        next: (user) => {
          this.applyUser(user);
        },
        error: () => {
          this.loadError.set('Unable to load profile. Please try again later.');
        },
      });
  }

  private applyUser(user: User): void {
    this.originalUser = user;
    this.name = user.name;
    this.surename = user.surename;
    this.email = user.email;
  }

  private resetFormFromOriginal(): void {
    if (!this.originalUser) {
      return;
    }

    this.name = this.originalUser.name;
    this.surename = this.originalUser.surename;
    this.email = this.originalUser.email;
    this.password = '';
    this.confirmPassword = '';
  }

  private buildUpdatePayload(): UpdateUserDto {
    const payload: UpdateUserDto = {};

    if (!this.originalUser) {
      return payload;
    }

    if (this.name !== this.originalUser.name) {
      payload.name = this.name;
    }

    if (this.surename !== this.originalUser.surename) {
      payload.surename = this.surename;
    }

    if (this.email !== this.originalUser.email) {
      payload.email = this.email;
    }

    if (this.changePassword() && this.password.trim()) {
      payload.password = this.password;
      payload.confirmPassword = this.confirmPassword;
    }

    return payload;
  }

}
