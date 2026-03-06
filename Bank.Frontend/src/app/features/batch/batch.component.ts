import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressBarModule } from '@angular/material/progress-bar';

@Component({
  selector: 'app-batch',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatButtonModule, MatProgressBarModule],
  template: `
    <div class="space-y-6">
      <!-- Page Header -->
      <div>
        <h1 class="text-3xl font-bold tracking-tight text-slate-900 dark:text-white">Batch Processing</h1>
        <p class="text-slate-500 dark:text-slate-400 mt-1">Upload and process multiple transactions at once.</p>
      </div>

      <!-- Main Action Area -->
      <div class="bg-white dark:bg-slate-900 rounded-3xl border border-slate-200 dark:border-slate-800 p-12 shadow-sm text-center">
        <div class="max-w-md mx-auto space-y-6">
          <div class="w-20 h-20 rounded-3xl bg-indigo-50 dark:bg-indigo-500/10 flex items-center justify-center text-indigo-600 dark:text-indigo-400 mx-auto">
            <mat-icon class="!w-10 !h-10 text-4xl">cloud_upload</mat-icon>
          </div>
          <div>
            <h2 class="text-xl font-bold text-slate-900 dark:text-white">Upload Batch File</h2>
            <p class="text-slate-500 dark:text-slate-400 mt-2">Drag and drop your CSV or Excel file here, or click to browse.</p>
          </div>
          <button mat-flat-button color="primary" class="!rounded-xl px-8 py-6">
            Select File
          </button>
        </div>
      </div>

      <!-- Active Batches -->
      <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
        <div *ngFor="let i of [1, 2]" class="bg-white dark:bg-slate-900 rounded-3xl border border-slate-200 dark:border-slate-800 p-6 shadow-sm">
          <div class="flex items-center justify-between mb-4">
            <div class="flex items-center space-x-3">
              <mat-icon class="text-slate-400">insert_drive_file</mat-icon>
              <span class="font-semibold text-slate-900 dark:text-white">batch_0942.csv</span>
            </div>
            <span class="text-xs font-bold px-2.5 py-1 rounded-full bg-amber-50 dark:bg-amber-500/10 text-amber-600 dark:text-amber-400">Processing</span>
          </div>
          <mat-progress-bar mode="determinate" [value]="i === 1 ? 65 : 30" class="!h-2 !rounded-full mb-2"></mat-progress-bar>
          <div class="flex justify-between text-xs text-slate-500 dark:text-slate-400">
            <span>{{ i === 1 ? '650' : '300' }} / 1000 processed</span>
            <span>{{ i === 1 ? '65%' : '30%' }}</span>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: `
    :host {
      display: block;
    }
    ::ng-deep .mat-mdc-progress-bar {
      --mdc-linear-progress-active-indicator-color: rgb(79 70 229);
      --mdc-linear-progress-track-color: rgb(241 245 249);
    }
    .dark ::ng-deep .mat-mdc-progress-bar {
      --mdc-linear-progress-track-color: rgb(30 41 59);
    }
  `
})
export class BatchComponent { }
