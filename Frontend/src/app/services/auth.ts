import { inject, Service } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map, Observable, of, switchMap, tap } from 'rxjs';
import { User, UpdateUserDto } from '../models/user.type';
import { AuthStore } from './auth-store';
import { environment } from '../environments/enviroment.development';

const TOKEN_STORAGE_KEY = 'token';

@Service()
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly authStore = inject(AuthStore);
  private readonly baseUrl = environment.apiUrl;

  login(dto: { email: string; password: string }): Observable<User> {
    return this.http
      .post<{ token: string }>(`${this.baseUrl}/Auth/login`, dto)
      .pipe(
        tap((res) => this.saveToken(res.token)),
        switchMap(() => this.getMe()),
        tap((user) => this.authStore.setUser(user)),
      );
  }

  register(dto: {
    name: string;
    surename: string;
    email: string;
    password: string;
    confirmPassword: string;
  }): Observable<User> {
    return this.http
      .post<{ token: string }>(`${this.baseUrl}/Auth/register`, dto)
      .pipe(
        tap((res) => this.saveToken(res.token)),
        switchMap(() => this.getMe()),
        tap((user) => this.authStore.setUser(user)),
      );
  }

  getMe(): Observable<User> {
    return this.http
      .get<{ user: User }>(`${this.baseUrl}/Users/me`)
      .pipe(map((res) => res.user));
  }

  updateUser(id: string, dto: UpdateUserDto): Observable<User> {
    return this.http
      .put<{ message: string }>(`${this.baseUrl}/Users/${id}`, dto)
      .pipe(
        switchMap(() => this.getMe()),
        tap((user) => this.authStore.setUser(user)),
      );
  }

  deleteUser(id: string): Observable<string> {
    return this.http
      .delete<{ message: string }>(`${this.baseUrl}/Users/${id}`)
      .pipe(
        map((res) => res.message),
        tap(() => this.logout()),
      );
  }

  
  initializeAuth(): Observable<User | null> {
  const token = this.getToken();

  if (!token) {
    if (this.authStore.isAuthenticated()) {
      this.authStore.logout();
    }
    return of(null);
  }

  return this.getMe().pipe(
    tap(user => this.authStore.setUser(user)),
    catchError(() => {
      this.logout();
      return of(null);
    })
  );
}

  saveToken(token: string): void {
    localStorage.setItem(TOKEN_STORAGE_KEY, token);
  }

  getToken(): string | null {
    return localStorage.getItem(TOKEN_STORAGE_KEY);
  }

  logout(): void {
    this.authStore.logout();
  }
}
