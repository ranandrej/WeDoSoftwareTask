import { Component, HostListener, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { ConfirmDeleteModal } from '../confirm-delete-modal/confirm-delete-modal';
import { AuthService } from '../../services/auth';
import { AuthStore } from '../../services/auth-store';

@Component({
  selector: 'app-header',
  imports: [RouterLink, ConfirmDeleteModal],
  templateUrl: './header.html',
  styleUrl: './header.css',
})
export class Header {
  protected readonly authStore = inject(AuthStore);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  userMenuOpen = signal(false);
  mobileMenuOpen = signal(false);
  logoutModalOpen = signal(false);

  toggleUserMenu(event: Event): void {
    event.stopPropagation();
    this.mobileMenuOpen.set(false);
    this.userMenuOpen.update((open) => !open);
  }

  toggleMobileMenu(event: Event): void {
    event.stopPropagation();
    this.closeUserMenu();
    this.mobileMenuOpen.update((open) => !open);
  }

  closeUserMenu(): void {
    this.userMenuOpen.set(false);
  }

  closeMobileMenu(): void {
    this.mobileMenuOpen.set(false);
  }

  @HostListener('document:click')
  onDocumentClick(): void {
    this.closeUserMenu();
    this.closeMobileMenu();
  }

  openLogoutModal(event: Event): void {
    event.stopPropagation();
    this.closeUserMenu();
    this.closeMobileMenu();
    this.logoutModalOpen.set(true);
  }

  confirmLogout(): void {
    this.logoutModalOpen.set(false);
    this.authService.logout();
    this.router.navigate(['/']);
  }

  closeLogoutModal(): void {
    this.logoutModalOpen.set(false);
  }
}
