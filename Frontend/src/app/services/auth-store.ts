import { computed, Service, signal } from '@angular/core';
import { User } from '../models/user.type';

const USER_STORAGE_KEY = 'user';

@Service()
export class AuthStore {
  private readonly currentUser = signal<User | null>(null);

  readonly user = this.currentUser.asReadonly();
  readonly isAuthenticated = computed(() => this.currentUser() !== null);

  constructor() {
    this.initializeFromStorage();
  }

  setUser(user: User): void {
    this.currentUser.set(user);
    localStorage.setItem(USER_STORAGE_KEY, JSON.stringify(user));
  }

  logout(): void {
    this.currentUser.set(null);
    localStorage.removeItem(USER_STORAGE_KEY);
    localStorage.removeItem('token');
  }

  private initializeFromStorage(): void {
    const savedUser = localStorage.getItem(USER_STORAGE_KEY);
    if (!savedUser) {
      return;
    }

    try {
      this.currentUser.set(JSON.parse(savedUser) as User);
    } catch {
      this.logout();
    }
  }
}
