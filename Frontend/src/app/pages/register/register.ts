import { Component, inject, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { finalize } from 'rxjs';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './register.html',
 
})
export class Register {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  name = '';
  surename = '';
  email = '';
  password = '';
  confirmPassword = '';
  loading = signal(false);
  successMessage = signal('');
  errorMessage = signal('');

  register(): void {
    this.loading.set(true);
    this.successMessage.set('');
    this.errorMessage.set('');

    this.authService
      .register({
        name: this.name,
        surename: this.surename,
        email: this.email,
        password: this.password,
        confirmPassword: this.confirmPassword,
      })
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: () => {
          this.successMessage.set('Registration successful!');
          this.router.navigate(['/workouts']);
        },
        error: (err: HttpErrorResponse) => {
          if (err.status === 503) {
            this.errorMessage.set('Too many requests. Please try again later.');
            return;
          }

          const message = typeof err.error === 'string' ? err.error : err.error?.error;
          this.errorMessage.set(message || 'Registration failed. Please try again.');
        },
      });
  }
}
