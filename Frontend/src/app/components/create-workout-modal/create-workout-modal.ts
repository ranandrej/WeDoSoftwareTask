import { Component, computed, effect, inject, input, output, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { finalize } from 'rxjs';
import { WorkoutService } from '../../services/workout.service';
import { CreateWorkoutDto, GetWorkout, WorkoutType } from '../../models/workout.type';

export type WorkoutFormMode = 'create' | 'update';

const MAX_DURATION_MINUTES = 120;
const MAX_CALORIES_BURNED = 2000;

@Component({
  selector: 'app-create-workout-modal',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './create-workout-modal.html',
  styleUrl: './create-workout-modal.css',
})
export class CreateWorkoutModal {
  private readonly workoutService = inject(WorkoutService);

  isOpen = input(false);
  mode = input<WorkoutFormMode>('create');
  workout = input<GetWorkout | null>(null);

  saved = output<string>();
  closed = output<void>();

  isUpdateMode = computed(() => this.mode() === 'update');

  types = signal<string[]>(Object.values(WorkoutType));
  loading = signal(false);
  errorMessage = signal('');

  name = '';
  type: WorkoutType = WorkoutType.Cardio;
  durationMinutes = 30;
  caloriesBurned = 0;
  difficulty = 5;
  fatigue = 5;
  notes = '';
  workoutDateTime = this.nowDateTimeLocal();

  constructor() {
    effect(() => {
      if (!this.isOpen()) {
        return;
      }

      this.loadTypes();

      if (this.isUpdateMode() && this.workout()) {
        this.populateForm(this.workout()!);
      } else {
        this.resetForm();
      }
    });
  }

  submit(): void {
    const trimmedName = this.name.trim();

    if (!trimmedName) {
      this.errorMessage.set('Name is required.');
      return;
    }

    const duration = Number(this.durationMinutes);
    const calories = Number(this.caloriesBurned);

    if (duration < 1 || duration > MAX_DURATION_MINUTES) {
      this.errorMessage.set(`Duration must be between 1 and ${MAX_DURATION_MINUTES} minutes.`);
      return;
    }

    if (calories < 0 || calories > MAX_CALORIES_BURNED) {
      this.errorMessage.set(`Calories burned must be between 0 and ${MAX_CALORIES_BURNED}.`);
      return;
    }

    const payload: CreateWorkoutDto = {
      name: trimmedName,
      type: this.type,
      durationMinutes: duration,
      caloriesBurned: calories,
      difficulty: Number(this.difficulty),
      fatigue: Number(this.fatigue),
      notes: this.notes.trim() || null,
      workoutDate: new Date(this.workoutDateTime).toISOString(),
    };

    const request$ =
      this.isUpdateMode() && this.workout()
        ? this.workoutService.update(this.workout()!.id, payload)
        : this.workoutService.create(payload);

    this.loading.set(true);
    this.errorMessage.set('');

    request$.pipe(finalize(() => this.loading.set(false))).subscribe({
      next: (message) => {
        this.saved.emit(message);
      },
      error: (err: HttpErrorResponse) => {
        this.errorMessage.set(this.extractErrorMessage(err));
      },
    });
  }

  onClose(): void {
    this.closed.emit();
  }

  private loadTypes(): void {
    this.workoutService.getTypes().subscribe({
      next: (types) => {
        if (types.length > 0) {
          this.types.set(types);
        }
      },
    });
  }

  private populateForm(workout: GetWorkout): void {
    this.name = workout.name;
    this.type = workout.type;
    this.durationMinutes = workout.durationMinutes;
    this.caloriesBurned = workout.caloriesBurned;
    this.difficulty = workout.difficulty;
    this.fatigue = workout.fatigue;
    this.notes = workout.notes ?? '';
    this.workoutDateTime = this.toDateTimeLocalValue(workout.workoutDate);
    this.errorMessage.set('');
  }

  private resetForm(): void {
    this.name = '';
    this.type = WorkoutType.Cardio;
    this.durationMinutes = 30;
    this.caloriesBurned = 0;
    this.difficulty = 5;
    this.fatigue = 5;
    this.notes = '';
    this.workoutDateTime = this.nowDateTimeLocal();
    this.errorMessage.set('');
  }

  private nowDateTimeLocal(): string {
    return this.toDateTimeLocalValue(new Date().toISOString());
  }

  private toDateTimeLocalValue(iso: string): string {
    const date = new Date(iso);
    const pad = (value: number) => value.toString().padStart(2, '0');

    return `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())}T${pad(date.getHours())}:${pad(date.getMinutes())}`;
  }

  private extractErrorMessage(err: HttpErrorResponse): string {
    if (typeof err.error === 'string') {
      return err.error;
    }

    if (err.error?.error) {
      return err.error.error;
    }

    return this.isUpdateMode()
      ? 'Unable to update workout. Please try again.'
      : 'Unable to create workout. Please try again.';
  }
}
