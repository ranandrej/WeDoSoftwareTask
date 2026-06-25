import { Component, input, output } from '@angular/core';

@Component({
  selector: 'app-confirm-delete-modal',
  standalone: true,
  templateUrl: './confirm-delete-modal.html',
})
export class ConfirmDeleteModal {
  isOpen = input(false);
  title = input('Delete account?');
  message = input(
    'Are you sure you want to delete your account? This action is permanent and all your workouts will be removed.',
  );
  cancelLabel = input('No, keep account');
  confirmLabel = input('Yes, delete account');

  confirmed = output<void>();
  cancelled = output<void>();

  onConfirm(): void {
    this.confirmed.emit();
  }

  onCancel(): void {
    this.cancelled.emit();
  }
}
