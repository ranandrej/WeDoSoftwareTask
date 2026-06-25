export type ChartMetric = 'workoutCount' | 'difficulty' | 'durationMinutes';

export const CHART_METRIC_LABELS: Record<ChartMetric, string> = {
  workoutCount: 'Workouts',
  difficulty: 'Avg. difficulty',
  durationMinutes: 'Duration (min)',
};
