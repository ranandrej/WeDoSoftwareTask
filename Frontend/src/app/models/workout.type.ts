
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

export type Workout = GetWorkout;
