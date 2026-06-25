import { Component, effect, inject, input, output, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { BaseChartDirective } from 'ng2-charts';
import { ChartConfiguration } from 'chart.js';
import { finalize } from 'rxjs';
import { WorkoutService } from '../../services/workout.service';
import { CHART_METRIC_LABELS, ChartMetric } from '../../models/chart.type';
import { MonthlyProgress } from '../../models/workout.type';

@Component({
  selector: 'app-monthly-progress-chart',
  standalone: true,
  imports: [BaseChartDirective],
  templateUrl: './monthly-progress-chart.html',
})
export class MonthlyProgressChart {
  private readonly workoutService = inject(WorkoutService);

  year = input.required<number>();
  month = input.required<number>();

  progressLoaded = output<MonthlyProgress>();
  errorChange = output<string>();

  selectedMetric = signal<ChartMetric>('workoutCount');
  progress = signal<MonthlyProgress | null>(null);
  loading = signal(false);
  error = signal('');
  hasData = signal(false);

  lineChartType: 'line' = 'line';
  lineChartData: ChartConfiguration<'line'>['data'] = {
    labels: [],
    datasets: [
      {
        data: [],
        label: 'Workouts',
        fill: true,
        tension: 0.3,
        borderColor: '#818cf8',
        backgroundColor: 'rgba(99, 102, 241, 0.2)',
        pointBackgroundColor: '#a5b4fc',
        pointBorderColor: '#e0e7ff',
      },
    ],
  };

  lineChartOptions: ChartConfiguration<'line'>['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: {
        labels: {
          color: '#cbd5e1',
        },
      },
    },
    scales: {
      x: {
        ticks: { color: '#94a3b8' },
        grid: { color: 'rgba(255, 255, 255, 0.08)' },
      },
      y: {
        beginAtZero: true,
        ticks: {
          color: '#94a3b8',
          stepSize: 1,
          precision: 0,
        },
        grid: { color: 'rgba(255, 255, 255, 0.08)' },
      },
    },
  };

  constructor() {
    effect(() => {
      this.loadProgress(this.year(), this.month());
    });

    effect(() => {
      const data = this.progress();
      const metric = this.selectedMetric();
      if (data) {
        this.updateChart(data, metric);
      }
    });
  }

  onMetricChange(value: string): void {
    this.selectedMetric.set(value as ChartMetric);
  }

  private loadProgress(year: number, month: number): void {
    this.loading.set(true);
    this.error.set('');
    this.errorChange.emit('');
    this.progress.set(null);
    this.hasData.set(false);

    this.workoutService
      .getMonthlyProgress(year, month)
      .pipe(
        finalize(() => {
          this.loading.set(false);
        }),
      )
      .subscribe({
        next: (loaded) => {
          this.progress.set(loaded);
          this.progressLoaded.emit(loaded);
        },
        error: (err: HttpErrorResponse) => {
          this.progress.set(null);
          this.hasData.set(false);
          const message = this.extractErrorMessage(err, 'Unable to load chart data.');
          this.error.set(message);
          this.errorChange.emit(message);
        },
      });
  }

  private updateChart(progress: MonthlyProgress, metric: ChartMetric): void {
    const labels = progress.weeks.map((week) => `Week ${week.weekNumber}`);

    const data = progress.weeks.map((week) => {
      switch (metric) {
        case 'difficulty':
          return week.averageDifficulty;
        case 'durationMinutes':
          return week.totalDurationMinutes;
        default:
          return week.workoutCount;
      }
    });

    const hasData = data.some((value) => value > 0);
    this.hasData.set(hasData);

    this.lineChartOptions = {
      ...this.lineChartOptions,
      scales: {
        ...this.lineChartOptions?.scales,
        y: {
          beginAtZero: true,
          ticks: {
            color: '#94a3b8',
            stepSize: metric === 'workoutCount' ? 1 : undefined,
            precision: metric === 'difficulty' ? 1 : 0,
          },
          grid: { color: 'rgba(255, 255, 255, 0.08)' },
        },
      },
    };

    this.lineChartData = {
      labels,
      datasets: [
        {
          ...this.lineChartData.datasets[0],
          data,
          label: CHART_METRIC_LABELS[metric],
        },
      ],
    };
  }

  private extractErrorMessage(err: HttpErrorResponse, fallback: string): string {
    if (typeof err.error === 'string') {
      return err.error;
    }

    if (err.error?.error) {
      return err.error.error;
    }

    return fallback;
  }
}
