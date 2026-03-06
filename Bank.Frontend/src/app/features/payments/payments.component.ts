import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-payments',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatButtonModule],
  template: `
    <div class="space-y-6">
      <!-- Page Header -->
      <div class="flex items-center justify-between">
        <div>
          <h1 class="text-3xl font-bold tracking-tight text-slate-900 dark:text-white">Payments</h1>
          <p class="text-slate-500 dark:text-slate-400 mt-1">Manage and track your financial transactions.</p>
        </div>
        <button mat-flat-button color="primary" class="!rounded-xl px-6 py-6">
          <mat-icon class="mr-2">add</mat-icon>
          New Payment
        </button>
      </div>

      <!-- Content Grid -->
      <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        <div class="md:col-span-2 bg-white dark:bg-slate-900 rounded-3xl border border-slate-200 dark:border-slate-800 p-8 shadow-sm">
          <div class="flex items-center space-x-4 mb-6">
            <div class="w-12 h-12 rounded-2xl bg-indigo-50 dark:bg-indigo-500/10 flex items-center justify-center text-indigo-600 dark:text-indigo-400">
              <mat-icon>send</mat-icon>
            </div>
            <div>
              <h3 class="text-lg font-bold text-slate-900 dark:text-white">Send Money</h3>
              <p class="text-sm text-slate-500 dark:text-slate-400">Transfer funds to other accounts instantly.</p>
            </div>
          </div>
          <div class="h-48 rounded-2xl border-2 border-dashed border-slate-200 dark:border-slate-800 flex items-center justify-center">
            <p class="text-slate-400 dark:text-slate-500 italic">Payment Form Coming Soon...</p>
          </div>
        </div>

        <div class="bg-white dark:bg-slate-900 rounded-3xl border border-slate-200 dark:border-slate-800 p-8 shadow-sm">
          <h3 class="text-lg font-bold text-slate-900 dark:text-white mb-6">Recent Activities</h3>
          <div class="space-y-4">
            <div *ngFor="let i of [1,2,3]" class="flex items-center space-x-4">
              <div class="w-10 h-10 rounded-full bg-slate-100 dark:bg-slate-800 flex-shrink-0"></div>
              <div class="flex-1 space-y-2">
                <div class="h-3 w-3/4 bg-slate-100 dark:bg-slate-800 rounded"></div>
                <div class="h-2 w-1/2 bg-slate-100 dark:bg-slate-800 rounded"></div>
              </div>
            </div>
          </div>
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
export class PaymentsComponent { }
