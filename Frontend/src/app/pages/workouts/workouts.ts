import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { finalize } from 'rxjs';
import { WorkoutService, WorkoutSort } from '../../services/workout.service';
import { GetWorkout } from '../../models/workout.type';
import {
  CreateWorkoutModal,
  WorkoutFormMode,
} from '../../components/create-workout-modal/create-workout-modal';
import { ConfirmDeleteModal } from '../../components/confirm-delete-modal/confirm-delete-modal';

@Component({
  selector: 'app-workouts',
  standalone: true,
  imports: [FormsModule, CreateWorkoutModal, ConfirmDeleteModal],
  templateUrl: './workouts.html',
  styleUrl: './workouts.css',
})
export class Workouts implements OnInit {
  private readonly workoutService = inject(WorkoutService);

  workouts = signal<GetWorkout[]>([]);
  pageLoading = signal(true);
  loadError = signal('');
  showWorkoutModal = signal(false);
  workoutModalMode = signal<WorkoutFormMode>('create');
  selectedWorkout = signal<GetWorkout | null>(null);
  showDeleteModal = signal(false);
  workoutToDelete = signal<GetWorkout | null>(null);
  deleting = signal(false);
  successMessage = signal('');
  actionError = signal('');
  selectedMonth = signal(this.currentMonthValue());
  sortOrder = signal<WorkoutSort>('desc');

  deleteModalMessage = computed(() => {
    const workout = this.workoutToDelete();
    if (!workout) {
      return 'Are you sure you want to delete this workout?';
    }

    return `Are you sure you want to delete "${workout.name}"? This action cannot be undone.`;
  });

  selectedMonthLabel = computed(() => {
    const [year, month] = this.selectedMonth().split('-').map(Number);
    return new Date(year, month - 1, 1).toLocaleDateString(undefined, {
      month: 'long',
      year: 'numeric',
    });
  });

  sortLabel = computed(() =>
    this.sortOrder() === 'desc' ? 'Newest first' : 'Oldest first',
  );

  ngOnInit(): void {
    this.loadWorkouts();
  }

  onMonthChange(value: string): void {
    this.selectedMonth.set(value);
    this.loadWorkouts();
  }

  toggleSort(): void {
    this.sortOrder.update((order) => (order === 'desc' ? 'asc' : 'desc'));
    this.loadWorkouts();
  }

  openCreateModal(): void {
    this.workoutModalMode.set('create');
    this.selectedWorkout.set(null);
    this.showWorkoutModal.set(true);
  }

  openUpdateModal(workout: GetWorkout): void {
    this.workoutModalMode.set('update');
    this.selectedWorkout.set(workout);
    this.showWorkoutModal.set(true);
  }

  closeWorkoutModal(): void {
    this.showWorkoutModal.set(false);
    this.selectedWorkout.set(null);
  }

  openDeleteModal(workout: GetWorkout): void {
    this.workoutToDelete.set(workout);
    this.showDeleteModal.set(true);
  }

  closeDeleteModal(): void {
    this.showDeleteModal.set(false);
    this.workoutToDelete.set(null);
  }

  onWorkoutSaved(message: string): void {
    this.showWorkoutModal.set(false);
    this.selectedWorkout.set(null);
    this.showSuccess(message || 'Workout saved successfully!');
    this.loadWorkouts();
  }

  onDeleteConfirmed(): void {
    const workout = this.workoutToDelete();
    if (!workout) {
      return;
    }

    this.showDeleteModal.set(false);
    this.deleting.set(true);

    this.workoutService
      .delete(workout.id)
      .pipe(finalize(() => this.deleting.set(false)))
      .subscribe({
        next: (message) => {
          this.workoutToDelete.set(null);
          this.showSuccess(message || 'Workout deleted successfully!');
          this.loadWorkouts();
        },
        error: (err: HttpErrorResponse) => {
          this.workoutToDelete.set(null);
          this.showActionError(this.extractErrorMessage(err));
        },
      });
  }

  formatDate(value: string): string {
    return new Date(value).toLocaleString(undefined, {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
    });
  }

  private loadWorkouts(): void {
    const [year, month] = this.selectedMonth().split('-').map(Number);

    this.pageLoading.set(true);
    this.loadError.set('');

    this.workoutService
      .getAll({ year, month, sort: this.sortOrder() })
      .pipe(finalize(() => this.pageLoading.set(false)))
      .subscribe({
        next: (workouts) => {
          this.workouts.set(workouts);
        },
        error: (err: HttpErrorResponse) => {
          this.loadError.set(this.extractErrorMessage(err, 'Unable to load workouts.'));
        },
      });
  }

  private currentMonthValue(): string {
    const now = new Date();
    const pad = (value: number) => value.toString().padStart(2, '0');
    return `${now.getFullYear()}-${pad(now.getMonth() + 1)}`;
  }

  private showSuccess(message: string): void {
    this.actionError.set('');
    this.successMessage.set(message);
    setTimeout(() => {
      this.successMessage.set('');
    }, 3000);
  }

  private showActionError(message: string): void {
    this.actionError.set(message);
    setTimeout(() => {
      this.actionError.set('');
    }, 3000);
  }

  private extractErrorMessage(err: HttpErrorResponse, fallback = 'Something went wrong.'): string {
    if (typeof err.error === 'string') {
      return err.error;
    }

    if (err.error?.error) {
      return err.error.error;
    }

    return fallback;
  }
}
