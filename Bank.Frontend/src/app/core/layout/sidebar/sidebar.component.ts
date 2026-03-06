import { Component, inject, signal } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatDividerModule } from '@angular/material/divider';
import { MatButtonModule } from '@angular/material/button';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { ThemeService } from '../../services/theme.service';
import { AuthService } from '../../services/auth.service';
import { LayoutService } from '../../services/layout.service';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive, MatIconModule, MatListModule, MatDividerModule, MatButtonModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css',
  animations: [
    trigger('sidebarCollapse', [
      state('expanded', style({ width: '256px' })),
      state('collapsed', style({ width: '80px' })),
      transition('expanded <=> collapsed', animate('300ms cubic-bezier(0.4, 0, 0.2, 1)'))
    ]),
    trigger('fadeInOut', [
      transition(':enter', [
        style({ opacity: 0, width: 0 }),
        animate('200ms ease-out', style({ opacity: 1, width: '*' }))
      ]),
      transition(':leave', [
        animate('150ms ease-in', style({ opacity: 0, width: 0 }))
      ])
    ])
  ],
  host: {
    '[@sidebarCollapse]': "isCollapsed ? 'collapsed' : 'expanded'",
    'class': 'block h-full border-r border-slate-200 dark:border-slate-800 transition-colors duration-500 flex-shrink-0'
  }
})
export class SidebarComponent {
  private authService = inject(AuthService);
  layoutService = inject(LayoutService);

  get isCollapsed() {
    return this.layoutService.isSidebarCollapsed();
  }

  navItems = [
    { icon: 'dashboard', label: 'Dashboard', route: '/dashboard' },
    { icon: 'payments', label: 'Payments', route: '/payments' },
    { icon: 'auto_awesome_motion', label: 'Batch Processing', route: '/batch' },
    { icon: 'group', label: 'Users & Accounts', route: '/users' },
    { icon: 'analytics', label: 'Reports', route: '/reports' },
    { icon: 'settings', label: 'Settings', route: '/settings' },
    { icon: 'account_circle', label: 'Profile', route: '/profile' },
    { icon: 'history_edu', label: 'Webhook Logs', route: '/logs' },
    { icon: 'menu_book', label: 'UI Docs', route: '/docs' },
  ];

  logout() {
    this.authService.logout();
  }
}

