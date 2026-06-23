import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  email = '';
  password = '';
  loading = signal(false);
  successMessage = signal('');
  errorMessage = signal('');

  login(): void {
    this.loading.set(true);
    this.successMessage.set('');
    this.errorMessage.set('');

    this.authService
      .login({ email: this.email, password: this.password })
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: () => {
          this.successMessage.set('Login successful!');
          this.router.navigate(['/workouts']);
        },
        error: () => {
          this.errorMessage.set('Invalid credentials. Please try again.');
        },
      });
  }
}
