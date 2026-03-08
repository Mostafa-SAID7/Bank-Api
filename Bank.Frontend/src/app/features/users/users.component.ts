import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatTabsModule } from '@angular/material/tabs';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';
import { UserService, User } from '../../core/services/user.service';
import { AccountService, Account } from '../../core/services/account.service';
import { forkJoin, catchError, of } from 'rxjs';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatButtonModule, MatMenuModule, MatTabsModule, MatProgressSpinnerModule, MatSnackBarModule],
  template: `
    <div class="space-y-6">
      <!-- Page Header -->
      <div class="flex items-center justify-between">
        <div>
          <h1 class="text-3xl font-bold tracking-tight text-slate-900 dark:text-white">Management</h1>
          <p class="text-slate-500 dark:text-slate-400 mt-1">Manage system users, accounts, and permissions.</p>
        </div>
        <div class="flex space-x-3">
          <button mat-flat-button color="primary" class="!rounded-xl px-6 py-6 font-bold shadow-lg shadow-indigo-200 dark:shadow-none">
            <mat-icon class="mr-2">person_add</mat-icon>
            Add User
          </button>
        </div>
      </div>

      <!-- Management Tabs -->
      <mat-tab-group class="custom-tabs">
        <!-- Users Tab -->
        <mat-tab label="Users">
          <div class="pt-6">
            <div class="bg-white dark:bg-slate-900 rounded-3xl border border-slate-200 dark:border-slate-800 overflow-hidden shadow-sm">
              <div class="overflow-x-auto text-nowrap">
                <table class="w-full text-left border-collapse">
                  <thead>
                    <tr class="bg-slate-50/50 dark:bg-slate-800/50 border-b border-slate-200 dark:border-slate-800">
                      <th class="px-6 py-4 text-sm font-bold text-slate-900 dark:text-white">User</th>
                      <th class="px-6 py-4 text-sm font-bold text-slate-900 dark:text-white">Email</th>
                      <th class="px-6 py-4 text-sm font-bold text-slate-900 dark:text-white">Status</th>
                      <th class="px-6 py-4 text-sm font-bold text-slate-900 dark:text-white text-right">Actions</th>
                    </tr>
                  </thead>
                  <tbody class="divide-y divide-slate-100 dark:divide-slate-800">
                    <tr *ngFor="let user of users" class="hover:bg-slate-50/50 dark:hover:bg-slate-800/20 transition-colors group">
                      <td class="px-6 py-4">
                        <div class="flex items-center space-x-3">
                          <div class="w-10 h-10 rounded-full bg-indigo-100 dark:bg-indigo-500/20 flex items-center justify-center text-indigo-600 dark:text-indigo-400 font-bold border border-indigo-200 dark:border-indigo-500/30">
                            {{ (user.firstName || user.userName).charAt(0).toUpperCase() }}
                          </div>
                          <div>
                            <div class="font-bold text-slate-900 dark:text-white">{{ user.firstName }} {{ user.lastName }}</div>
                            <div class="text-xs text-slate-500 dark:text-slate-400 truncate max-w-[150px]">&#64;{{ user.userName }}</div>
                          </div>
                        </div>
                      </td>
                      <td class="px-6 py-4">
                        <span class="text-sm text-slate-600 dark:text-slate-300 font-medium">{{ user.email }}</span>
                      </td>
                      <td class="px-6 py-4">
                        <span class="inline-flex items-center px-2.5 py-1 rounded-lg text-xs font-bold leading-none bg-emerald-50 dark:bg-emerald-500/10 text-emerald-600 dark:text-emerald-400 border border-emerald-100 dark:border-emerald-500/20">
                          Active
                        </span>
                      </td>
                      <td class="px-6 py-4 text-right">
                        <button mat-icon-button [matMenuTriggerFor]="menu" class="text-slate-400 hover:text-indigo-600 dark:hover:text-indigo-400 transition-colors" [disabled]="actionLoading === user.id">
                          <mat-icon *ngIf="actionLoading !== user.id">more_vert</mat-icon>
                          <mat-spinner *ngIf="actionLoading === user.id" diameter="20"></mat-spinner>
                        </button>
                        <mat-menu #menu="matMenu" class="!rounded-2xl !p-2 !shadow-xl !border !border-slate-100 dark:!border-slate-800">
                          <button mat-menu-item class="!rounded-xl">
                            <mat-icon>edit</mat-icon>
                            <span>Edit User</span>
                          </button>
                          <button mat-menu-item class="!rounded-xl text-rose-500" (click)="suspendUser(user.id)">
                            <mat-icon class="!text-rose-500">delete_outline</mat-icon>
                            <span>Suspend</span>
                          </button>
                        </mat-menu>
                      </td>
                    </tr>
                    <tr *ngIf="users.length === 0 && !loading">
                      <td colspan="4" class="px-6 py-12 text-center text-slate-400 italic">No users found.</td>
                    </tr>
                  </tbody>
                </table>
              </div>
            </div>
          </div>
        </mat-tab>

        <!-- Accounts Tab -->
        <mat-tab label="Accounts">
          <div class="pt-6">
            <div class="bg-white dark:bg-slate-900 rounded-3xl border border-slate-200 dark:border-slate-800 overflow-hidden shadow-sm">
              <div class="overflow-x-auto text-nowrap">
                <table class="w-full text-left border-collapse">
                  <thead>
                    <tr class="bg-slate-50/50 dark:bg-slate-800/50 border-b border-slate-200 dark:border-slate-800">
                      <th class="px-6 py-4 text-sm font-bold text-slate-900 dark:text-white">Account Number</th>
                      <th class="px-6 py-4 text-sm font-bold text-slate-900 dark:text-white">Holder Name</th>
                      <th class="px-6 py-4 text-sm font-bold text-slate-900 dark:text-white text-right">Balance</th>
                      <th class="px-6 py-4 text-sm font-bold text-slate-900 dark:text-white text-right">Actions</th>
                    </tr>
                  </thead>
                  <tbody class="divide-y divide-slate-100 dark:divide-slate-800">
                    <tr *ngFor="let acc of accounts" class="hover:bg-slate-50/50 dark:hover:bg-slate-800/20 transition-colors">
                      <td class="px-6 py-4">
                        <div class="flex items-center space-x-2">
                          <mat-icon class="text-slate-400 text-sm">account_balance_wallet</mat-icon>
                          <span class="text-sm font-mono font-bold text-slate-900 dark:text-white">{{ acc.accountNumber }}</span>
                        </div>
                      </td>
                      <td class="px-6 py-4">
                        <div class="font-bold text-slate-900 dark:text-white">{{ acc.accountHolderName }}</div>
                      </td>
                      <td class="px-6 py-4 text-right">
                        <div class="text-sm font-bold text-emerald-600 dark:text-emerald-400">
                          {{ acc.balance | currency }}
                        </div>
                      </td>
                      <td class="px-6 py-4 text-right">
                        <button mat-icon-button [matMenuTriggerFor]="accMenu" class="text-slate-400 hover:text-indigo-600 dark:hover:text-indigo-400 transition-colors" [disabled]="actionLoading === acc.id">
                          <mat-icon *ngIf="actionLoading !== acc.id">more_vert</mat-icon>
                          <mat-spinner *ngIf="actionLoading === acc.id" diameter="20"></mat-spinner>
                        </button>
                        <mat-menu #accMenu="matMenu" class="!rounded-2xl !p-2">
                          <button mat-menu-item class="!rounded-xl">
                            <mat-icon>history</mat-icon>
                            <span>Transactions</span>
                          </button>
                          <button mat-menu-item class="!rounded-xl text-amber-500" (click)="freezeAccount(acc.id)">
                            <mat-icon class="!text-amber-500">lock_outline</mat-icon>
                            <span>Freeze</span>
                          </button>
                        </mat-menu>
                      </td>
                    </tr>
                    <tr *ngIf="accounts.length === 0 && !loading">
                      <td colspan="4" class="px-6 py-12 text-center text-slate-400 italic">No accounts found.</td>
                    </tr>
                  </tbody>
                </table>
              </div>
            </div>
          </div>
        </mat-tab>
      </mat-tab-group>

      <div *ngIf="loading" class="flex justify-center py-12">
        <mat-spinner diameter="40"></mat-spinner>
      </div>
    </div>
  `,
  styles: [`
    :host {
      display: block;
    }
    ::ng-deep .custom-tabs .mat-mdc-tab-labels {
      @apply bg-slate-100/50 dark:bg-slate-800/30 p-1 rounded-2xl w-fit border border-slate-200 dark:border-slate-800;
    }
    ::ng-deep .custom-tabs .mat-mdc-tab {
      @apply !h-10 !rounded-xl transition-all;
    }
    ::ng-deep .custom-tabs .mat-mdc-tab.mdc-tab--active {
      @apply bg-white dark:bg-slate-900 shadow-sm;
    }
    ::ng-deep .custom-tabs .mat-mdc-tab-indicator {
      display: none;
    }
  `]
})
export class UsersComponent implements OnInit {
  private userService = inject(UserService);
  private accountService = inject(AccountService);
  private snackBar = inject(MatSnackBar);

  users: User[] = [];
  accounts: Account[] = [];
  loading = false;
  actionLoading: string | null = null;

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.loading = true;

    // Better synchronization for parallel API calls
    forkJoin({
      users: this.userService.getAllUsers().pipe(catchError(() => of([]))),
      accounts: this.accountService.getAllAccounts().pipe(catchError(() => of([])))
    }).subscribe({
      next: (data) => {
        this.users = data.users;
        this.accounts = data.accounts;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.snackBar.open('Failed to load management data', 'Close', { duration: 3000 });
      }
    });
  }

  suspendUser(userId: string) {
    if (confirm('Are you sure you want to suspend this user?')) {
      this.actionLoading = userId;
      this.userService.suspendUser(userId).subscribe({
        next: () => {
          this.snackBar.open('User suspended successfully focus.', 'Close', { duration: 3000 });
          // Remove from local list manually to reflect UI change rapidly
          this.users = this.users.filter(u => u.id !== userId);
          this.actionLoading = null;
        },
        error: () => {
          this.snackBar.open('Failed to suspend user.', 'Close', { duration: 3000 });
          this.actionLoading = null;
        }
      });
    }
  }

  freezeAccount(accountId: string) {
    if (confirm('Are you sure you want to freeze this account?')) {
      this.actionLoading = accountId;
      // Here we might normally call a separate freeze endpoint, but for
      // this demonstration we just mock the success message
      setTimeout(() => {
        this.snackBar.open('Account frozen successfully.', 'Close', { duration: 3000 });
        this.actionLoading = null;
      }, 800);
    }
  }
}
