import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';

@Component({
  selector: 'app-reports',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatButtonModule, MatSelectModule, MatFormFieldModule],
  template: `
    <div class="space-y-6">
      <!-- Page Header -->
      <div class="flex items-center justify-between">
        <div>
          <h1 class="text-3xl font-bold tracking-tight text-slate-900 dark:text-white">Reports & Analytics</h1>
          <p class="text-slate-500 dark:text-slate-400 mt-1">Generate and view detailed financial reports.</p>
        </div>
        <div class="flex items-center space-x-3">
          <button mat-stroked-button class="!rounded-xl px-4 py-6 !border-slate-200 dark:!border-slate-800">
            <mat-icon class="mr-2">file_download</mat-icon>
            Export PDF
          </button>
          <button mat-flat-button color="primary" class="!rounded-xl px-4 py-6">
            <mat-icon class="mr-2">analytics</mat-icon>
            Generate New
          </button>
        </div>
      </div>

      <!-- Quick Stats -->
      <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
        <div *ngFor="let stat of stats" class="bg-white dark:bg-slate-900 rounded-3xl border border-slate-200 dark:border-slate-800 p-6 shadow-sm">
          <div class="flex items-center justify-between mb-4">
            <div class="w-10 h-10 rounded-2xl flex items-center justify-center" [class]="stat.bg">
              <mat-icon [class]="stat.color">{{ stat.icon }}</mat-icon>
            </div>
            <span class="text-xs font-bold text-emerald-500 flex items-center">
              <mat-icon class="text-xs mr-0.5">trending_up</mat-icon>
              {{ stat.trend }}
            </span>
          </div>
          <div class="text-2xl font-bold text-slate-900 dark:text-white">{{ stat.value }}</div>
          <div class="text-sm text-slate-500 dark:text-slate-400 mt-1">{{ stat.label }}</div>
        </div>
      </div>

      <!-- Charts Area Placeholder -->
      <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <div class="bg-white dark:bg-slate-900 rounded-3xl border border-slate-200 dark:border-slate-800 p-8 shadow-sm">
          <h3 class="text-lg font-bold text-slate-900 dark:text-white mb-6">Revenue Growth</h3>
          <div class="h-64 rounded-2xl bg-slate-50 dark:bg-slate-800/50 flex items-center justify-center border-2 border-dashed border-slate-200 dark:border-slate-800">
            <p class="text-slate-400 dark:text-slate-500 italic">Chart Visualization Coming Soon...</p>
          </div>
        </div>
        <div class="bg-white dark:bg-slate-900 rounded-3xl border border-slate-200 dark:border-slate-800 p-8 shadow-sm">
          <h3 class="text-lg font-bold text-slate-900 dark:text-white mb-6">Transaction Distribution</h3>
          <div class="h-64 rounded-2xl bg-slate-50 dark:bg-slate-800/50 flex items-center justify-center border-2 border-dashed border-slate-200 dark:border-slate-800">
            <p class="text-slate-400 dark:text-slate-500 italic">Chart Visualization Coming Soon...</p>
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
export class ReportsComponent {
  stats = [
    { label: 'Total Revenue', value: '$84,232', trend: '12%', icon: 'payments', bg: 'bg-emerald-50 dark:bg-emerald-500/10', color: 'text-emerald-600 dark:text-emerald-400' },
    { label: 'Active Users', value: '1,284', trend: '8%', icon: 'groups', bg: 'bg-indigo-50 dark:bg-indigo-500/10', color: 'text-indigo-600 dark:text-indigo-400' },
    { label: 'Growth Rate', value: '24.5%', trend: '15%', icon: 'show_chart', bg: 'bg-amber-50 dark:bg-amber-500/10', color: 'text-amber-600 dark:text-amber-400' },
  ];
}
