import { inject, Service } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { CreateWorkoutDto, GetWorkout, MonthlyProgress } from '../models/workout.type';
import { environment } from '../environments/enviroment.development';

export type WorkoutSort = 'asc' | 'desc';

export type GetWorkoutsParams = {
  year: number;
  month: number;
  sort?: WorkoutSort;
};

@Service()
export class WorkoutService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiUrl;

  getTypes(): Observable<string[]> {
    return this.http
      .get<{ types: string[] }>(`${this.baseUrl}/Workouts/types`)
      .pipe(map((res) => res.types));
  }

  getAll(params: GetWorkoutsParams): Observable<GetWorkout[]> {
    return this.http
      .get<{ workouts: GetWorkout[] }>(`${this.baseUrl}/Workouts/all`, {
        params: {
          year: params.year,
          month: params.month,
          sort: params.sort ?? 'desc',
        },
      })
      .pipe(map((res) => res.workouts));
  }

  getById(id: string): Observable<GetWorkout> {
    return this.http
      .get<{ workout: GetWorkout }>(`${this.baseUrl}/Workouts/${id}`)
      .pipe(map((res) => res.workout));
  }

  create(dto: CreateWorkoutDto): Observable<string> {
    return this.http
      .post<{ message: string }>(`${this.baseUrl}/Workouts/create`, dto)
      .pipe(map((res) => res.message));
  }

  update(id: string, dto: CreateWorkoutDto): Observable<string> {
    return this.http
      .put<{ message: string }>(`${this.baseUrl}/Workouts/${id}`, dto)
      .pipe(map((res) => res.message));
  }

  delete(id: string): Observable<string> {
    return this.http
      .delete<{ message: string }>(`${this.baseUrl}/Workouts/${id}`)
      .pipe(map((res) => res.message));
  }

  getMonthlyProgress(year: number, month: number): Observable<MonthlyProgress> {
    return this.http
      .get<{ progress: MonthlyProgress }>(`${this.baseUrl}/Workouts/progress`, {
        params: { year, month },
      })
      .pipe(map((res) => res.progress));
  }

  getMonthlyProgressPdf(year: number, month: number): Observable<Blob> {
    return this.http.get(`${this.baseUrl}/Workouts/progress-pdf`, {
      params: { year, month },
      responseType: 'blob',
    });
  }
}
