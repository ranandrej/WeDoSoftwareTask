import { Component, input, output } from '@angular/core';

@Component({
  selector: 'app-confirm-delete-modal',
  standalone: true,
  templateUrl: './confirm-delete-modal.html',
})
export class ConfirmDeleteModal {
  isOpen = input(false);

  confirmed = output<void>();
  cancelled = output<void>();

  onConfirm(): void {
    this.confirmed.emit();
  }

  onCancel(): void {
    this.cancelled.emit();
  }
}
