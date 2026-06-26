import { Component, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { WorkoutService } from '../../services/workout.service';
import { MonthlyProgress } from '../../models/workout.type';
import { MonthlyProgressChart } from '../../components/monthly-progress-chart/monthly-progress-chart';

@Component({
  selector: 'app-progress',
  standalone: true,
  imports: [FormsModule, MonthlyProgressChart],
  templateUrl: './progress.html',
})
export class Progress {
  private readonly workoutService = inject(WorkoutService);

  selectedMonth = signal(this.currentMonthValue());
  monthlyProgress = signal<MonthlyProgress | null>(null);
  actionError = signal('');
  pdfLoading = signal(false);

  selectedYear = computed(() => {
    const [year] = this.selectedMonth().split('-').map(Number);
    return year;
  });

  selectedMonthNumber = computed(() => {
    const [, month] = this.selectedMonth().split('-').map(Number);
    return month;
  });
  selectedMonthTotalWorkouts = computed(() => {
    const progress = this.monthlyProgress();
    if (!progress) {
      return 0;
    }
    return progress.weeks.reduce((acc, week) => acc + week.workoutCount, 0);
  });
  hasWorkouts = computed(() => {
    const progress = this.monthlyProgress();
    if (!progress) {
      return false;
    }

    return progress.weeks.some((week) => week.workoutCount > 0);
  });

  selectedMonthLabel = computed(() => {
    const progress = this.monthlyProgress();
    if (!progress) {
      const [year, month] = this.selectedMonth().split('-').map(Number);
      return new Date(year, month - 1, 1).toLocaleDateString(undefined, {
        month: 'long',
        year: 'numeric',
      });
    }

    return new Date(progress.year, progress.month - 1, 1).toLocaleDateString(undefined, {
      month: 'long',
      year: 'numeric',
    });
  });

  onMonthChange(value: string): void {
    this.selectedMonth.set(value);
    this.monthlyProgress.set(null);
  }

  onProgressLoaded(progress: MonthlyProgress): void {
    this.monthlyProgress.set(progress);
  }

  onProgressErrorChange(error: string): void {
    if (error) {
      this.monthlyProgress.set(null);
    }
  }

  downloadProgressPdf(): void {
    const [year, month] = this.selectedMonth().split('-').map(Number);

    this.pdfLoading.set(true);

    this.workoutService.getMonthlyProgressPdf(year, month).subscribe({
      next: (pdf: Blob) => {
        const url = window.URL.createObjectURL(pdf);

        const a = document.createElement('a');
        a.href = url;
        a.download = `monthly-progress-${year}-${month}.pdf`;
        document.body.appendChild(a);
        a.click();
        a.remove();
        window.URL.revokeObjectURL(url);
        this.pdfLoading.set(false);
      },
      error: () => {
        this.pdfLoading.set(false);
        this.showActionError('Failed to download progress PDF. Please try again later.');
      },
      complete: () => {
        this.pdfLoading.set(false);
      },
    });
  }

  formatAverage(value: number): string {
    return value > 0 ? value.toFixed(2) : '—';
  }

  private currentMonthValue(): string {
    const now = new Date();
    const pad = (value: number) => value.toString().padStart(2, '0');
    return `${now.getFullYear()}-${pad(now.getMonth() + 1)}`;
  }

  private showActionError(message: string): void {
    this.actionError.set(message);
    setTimeout(() => {
      this.actionError.set('');
    }, 3000);
  }
}
