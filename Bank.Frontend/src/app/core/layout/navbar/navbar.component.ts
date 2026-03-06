import { Component, inject, signal, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatBadgeModule } from '@angular/material/badge';
import { trigger, transition, style, animate, state } from '@angular/animations';
import { ThemeService } from '../../services/theme.service';
import { AuthService } from '../../services/auth.service';
import { LayoutService } from '../../services/layout.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    CommonModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatInputModule,
    MatFormFieldModule,
    MatBadgeModule
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
  animations: [
    trigger('dropdownAnimation', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(-10px) scale(0.95)' }),
        animate('200ms cubic-bezier(0.4, 0, 0.2, 1)',
          style({ opacity: 1, transform: 'translateY(0) scale(1)' }))
      ]),
      transition(':leave', [
        animate('150ms cubic-bezier(0.4, 0, 0.2, 1)',
          style({ opacity: 0, transform: 'translateY(-10px) scale(0.95)' }))
      ])
    ])
  ]
})
export class NavbarComponent {
  themeService = inject(ThemeService);
  authService = inject(AuthService);
  layoutService = inject(LayoutService);

  showSearchSuggestions = signal(false);
  showNotifications = signal(false);
  showProfileMenu = signal(false);

  searchQuery = '';

  suggestions = [
    { type: 'Account', title: 'Main Savings Account', detail: '...4492', icon: 'account_balance_wallet' },
    { type: 'Transaction', title: 'WPS Salary Batch Feb', detail: 'Processing', icon: 'payments' },
    { type: 'User', title: 'Hamad Al-Mulla', detail: 'Manager', icon: 'person' },
  ];

  notifications = [
    { title: 'New Salary Batch', time: '5m ago', type: 'info', icon: 'info' },
    { title: 'ACH Return Alert', time: '1h ago', type: 'warning', icon: 'warning' },
    { title: 'System Update', time: '3h ago', type: 'success', icon: 'check_circle' },
  ];

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    const target = event.target as HTMLElement;
    if (!target.closest('.search-container')) this.showSearchSuggestions.set(false);
    if (!target.closest('.notifications-container')) this.showNotifications.set(false);
    if (!target.closest('.profile-container')) this.showProfileMenu.set(false);
  }

  toggleTheme() {
    this.themeService.toggleTheme();
  }

  logout() {
    this.authService.logout();
  }
}
