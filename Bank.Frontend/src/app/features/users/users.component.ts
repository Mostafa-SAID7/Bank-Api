import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatButtonModule, MatMenuModule],
  template: `
    <div class="space-y-6">
      <!-- Page Header -->
      <div class="flex items-center justify-between">
        <div>
          <h1 class="text-3xl font-bold tracking-tight text-slate-900 dark:text-white">Users & Accounts</h1>
          <p class="text-slate-500 dark:text-slate-400 mt-1">Manage system users, roles, and account permissions.</p>
        </div>
        <button mat-flat-button color="primary" class="!rounded-xl px-6 py-6">
          <mat-icon class="mr-2">person_add</mat-icon>
          Add User
        </button>
      </div>

      <!-- Users Table Card -->
      <div class="bg-white dark:bg-slate-900 rounded-3xl border border-slate-200 dark:border-slate-800 overflow-hidden shadow-sm">
        <div class="overflow-x-auto">
          <table class="w-full text-left border-collapse">
            <thead>
              <tr class="bg-slate-50/50 dark:bg-slate-800/50 border-b border-slate-200 dark:border-slate-800">
                <th class="px-6 py-4 text-sm font-bold text-slate-900 dark:text-white">User</th>
                <th class="px-6 py-4 text-sm font-bold text-slate-900 dark:text-white">Role</th>
                <th class="px-6 py-4 text-sm font-bold text-slate-900 dark:text-white">Status</th>
                <th class="px-6 py-4 text-sm font-bold text-slate-900 dark:text-white text-right">Actions</th>
              </tr>
            </thead>
            <tbody class="divide-y divide-slate-100 dark:divide-slate-800">
              <tr *ngFor="let user of users" class="hover:bg-slate-50/50 dark:hover:bg-slate-800/20 transition-colors">
                <td class="px-6 py-4">
                  <div class="flex items-center space-x-3">
                    <div class="w-10 h-10 rounded-full bg-indigo-100 dark:bg-indigo-500/20 flex items-center justify-center text-indigo-600 dark:text-indigo-400 font-bold">
                      {{ user.name.charAt(0) }}
                    </div>
                    <div>
                      <div class="font-bold text-slate-900 dark:text-white">{{ user.name }}</div>
                      <div class="text-xs text-slate-500 dark:text-slate-400">{{ user.email }}</div>
                    </div>
                  </div>
                </td>
                <td class="px-6 py-4">
                  <span class="text-xs font-semibold px-2.5 py-1 rounded-lg bg-slate-100 dark:bg-slate-800 text-slate-600 dark:text-slate-300">
                    {{ user.role }}
                  </span>
                </td>
                <td class="px-6 py-4">
                  <span class="flex items-center space-x-1.5" [class.text-emerald-500]="user.status === 'Active'" [class.text-slate-400]="user.status === 'Inactive'">
                    <span class="w-1.5 h-1.5 rounded-full bg-current"></span>
                    <span class="text-xs font-bold">{{ user.status }}</span>
                  </span>
                </td>
                <td class="px-6 py-4 text-right">
                  <button mat-icon-button [matMenuTriggerFor]="menu" class="text-slate-400">
                    <mat-icon>more_vert</mat-icon>
                  </button>
                  <mat-menu #menu="matMenu" class="!rounded-2xl !p-2">
                    <button mat-menu-item class="!rounded-xl">
                      <mat-icon>edit</mat-icon>
                      <span>Edit User</span>
                    </button>
                    <button mat-menu-item class="!rounded-xl text-rose-500">
                      <mat-icon class="!text-rose-500">delete</mat-icon>
                      <span>Remove</span>
                    </button>
                  </mat-menu>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  `,
  styles: `
    :host {
      display: block;
    }
  `
})
export class UsersComponent {
  users = [
    { name: 'Ahmed Ali', email: 'ahmed@finbank.com', role: 'Administrator', status: 'Active' },
    { name: 'Sara Hassan', email: 'sara@finbank.com', role: 'Compliance Officer', status: 'Active' },
    { name: 'Omar Mahmoud', email: 'omar@finbank.com', role: 'Support Agent', status: 'Inactive' },
    { name: 'Laila Karim', email: 'laila@finbank.com', role: 'Finance Manager', status: 'Active' },
  ];
}
