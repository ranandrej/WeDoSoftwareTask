
//Workout potrebni tipovi

export enum WorkoutType {
  Cardio = 'Cardio',
  Strength = 'Strength',
  Flexibility = 'Flexibility',
}


export type GetWorkout = {
  id: string;
  name: string;
  type: WorkoutType;
  durationMinutes: number;
  caloriesBurned: number;
  difficulty: number;
  fatigue: number;
  notes: string | null;
  workoutDate: string;
};

export type CreateWorkoutDto = {
  name: string;
  type: WorkoutType;
  durationMinutes: number;
  caloriesBurned: number;
  difficulty: number;
  fatigue: number;
  notes?: string | null;
  workoutDate: string;
};

export type WeeklyProgress = {
  weekNumber: number;
  totalDurationMinutes: number;
  workoutCount: number;
  averageDifficulty: number;
  averageFatigue: number;
};

export type MonthlyProgress = {
  year: number;
  month: number;
  weeks: WeeklyProgress[];
};

export type Workout = GetWorkout;
