import { Component, OnInit, inject, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';
import { BatchService, BatchJob } from '../../core/services/batch.service';
import { interval, Subscription, switchMap, startWith, catchError, of } from 'rxjs';

@Component({
  selector: 'app-batch',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatButtonModule, MatProgressBarModule, MatSnackBarModule],
  template: `
    <div class="space-y-6 max-w-5xl mx-auto">
      <!-- Page Header -->
      <div class="flex items-center justify-between">
        <div>
          <h1 class="text-3xl font-bold tracking-tight text-slate-900 dark:text-white">Batch Processing</h1>
          <p class="text-slate-500 dark:text-slate-400 mt-1">Efficiently process large volumes of transactions via file upload.</p>
        </div>
      </div>

      <!-- Main Upload Area -->
      <div class="bg-white dark:bg-slate-900 rounded-3xl border-2 border-dashed border-slate-200 dark:border-slate-800 p-12 shadow-sm text-center transition-all hover:border-indigo-400 dark:hover:border-indigo-500/50 group">
        <div class="max-w-md mx-auto space-y-6">
          <div class="w-24 h-24 rounded-3xl bg-indigo-50 dark:bg-indigo-500/10 flex items-center justify-center text-indigo-600 dark:text-indigo-400 mx-auto group-hover:scale-110 transition-transform duration-300">
            <mat-icon class="!w-12 !h-12 text-5xl">cloud_upload</mat-icon>
          </div>
          <div>
            <h2 class="text-2xl font-bold text-slate-900 dark:text-white">Upload Batch File</h2>
            <p class="text-slate-500 dark:text-slate-400 mt-2">Process thousands of records in seconds. Supports CSV and JSON formats.</p>
          </div>
          
          <input type="file" #fileInput (change)="onFileSelected($event)" accept=".csv,.json" class="hidden">
          
          <div class="flex flex-col items-center space-y-4">
            <button mat-flat-button color="primary" class="!rounded-2xl !px-10 !py-8 !text-lg !font-bold" (click)="fileInput.click()" [disabled]="uploading">
              <span *ngIf="!uploading">Select Batch File</span>
              <span *ngIf="uploading">Uploading...</span>
            </button>
            <p class="text-xs text-slate-400 tracking-wider font-semibold uppercase">Maximum file size: 25MB</p>
          </div>
        </div>
      </div>

      <!-- Active & Recent Batches -->
      <div class="space-y-4">
        <h3 class="text-lg font-bold text-slate-900 dark:text-white flex items-center">
          <mat-icon class="mr-2 text-indigo-500">pending_actions</mat-icon>
          Active & Recent Batches
        </h3>

        <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div *ngFor="let job of activeJobs" class="bg-white dark:bg-slate-900 rounded-3xl border border-slate-200 dark:border-slate-800 p-6 shadow-sm hover:shadow-md transition-shadow">
            <div class="flex items-center justify-between mb-4">
              <div class="flex items-center space-x-3">
                <div class="w-10 h-10 rounded-xl bg-slate-50 dark:bg-slate-800 flex items-center justify-center text-slate-400">
                  <mat-icon>insert_drive_file</mat-icon>
                </div>
                <div>
                  <span class="font-bold text-slate-900 dark:text-white block">{{ job.fileName }}</span>
                  <span class="text-xs text-slate-500">{{ job.createdAt | date:'short' }}</span>
                </div>
              </div>
              <span class="text-[10px] font-bold px-2.5 py-1 rounded-full uppercase tracking-wider"
                    [ngClass]="{
                      'bg-amber-50 dark:bg-amber-500/10 text-amber-600 dark:text-amber-400': job.status === 'Processing',
                      'bg-emerald-50 dark:bg-emerald-500/10 text-emerald-600 dark:text-emerald-400': job.status === 'Completed',
                      'bg-rose-50 dark:bg-rose-500/10 text-rose-600 dark:text-rose-400': job.status === 'Failed'
                    }">
                {{ job.status }}
              </span>
            </div>
            
            <mat-progress-bar mode="determinate" [value]="(job.processedRecords / job.totalRecords) * 100" class="!h-2.5 !rounded-full mb-3"></mat-progress-bar>
            
            <div class="flex justify-between text-xs font-bold">
              <span class="text-slate-500">{{ job.processedRecords }} / {{ job.totalRecords }} processed</span>
              <span class="text-indigo-600 dark:text-indigo-400">{{ ((job.processedRecords / job.totalRecords) * 100) | number:'1.0-0' }}%</span>
            </div>
          </div>

          <div *ngIf="activeJobs.length === 0" class="md:col-span-2 py-12 text-center bg-slate-50/50 dark:bg-slate-800/20 rounded-3xl border border-dashed border-slate-200 dark:border-slate-800">
            <mat-icon class="text-slate-300 dark:text-slate-700 !w-12 !h-12 text-5xl mb-2">history</mat-icon>
            <p class="text-slate-400 dark:text-slate-500 italic">No active or recent batch jobs found.</p>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
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
  `]
})
export class BatchComponent implements OnInit, OnDestroy {
  private batchService = inject(BatchService);
  private snackBar = inject(MatSnackBar);
  private statusSub?: Subscription;

  activeJobs: BatchJob[] = [];
  uploading = false;

  ngOnInit() {
    this.startPolling();
  }

  ngOnDestroy() {
    this.statusSub?.unsubscribe();
  }

  onFileSelected(event: any) {
    const file: File = event.target.files[0];
    if (file) {
      this.uploadFile(file);
    }
  }

  private uploadFile(file: File) {
    this.uploading = true;
    this.batchService.uploadBatch(file).subscribe({
      next: (res) => {
        this.uploading = false;
        this.snackBar.open('Batch file uploaded successfully!', 'Close', { duration: 3000 });
        this.loadJobStatus(res.jobId);
      },
      error: (err) => {
        this.uploading = false;
        this.snackBar.open('Upload failed. Please try again.', 'Close', { duration: 5000 });
      }
    });
  }

  private startPolling() {
    this.batchService.getAllBatches().subscribe({
      next: (jobs) => {
        this.activeJobs = jobs;
        // Find any jobs that are still processing and actively poll them
        const processingJobs = jobs.filter(j => j.status === 'Pending' || j.status === 'Processing');
        processingJobs.forEach(job => this.loadJobStatus(job.id));
      },
      error: () => console.error('Failed to load batch history')
    });
  }

  private loadJobStatus(jobId: string) {
    // Start polling for this specific job
    const jobSub = interval(2000).pipe(
      startWith(0),
      switchMap(() => this.batchService.getBatchStatus(jobId)),
      catchError(() => of(null))
    ).subscribe(job => {
      if (job) {
        const index = this.activeJobs.findIndex(j => j.id === job.id);
        if (index > -1) {
          this.activeJobs[index] = job;
        } else {
          this.activeJobs.unshift(job);
        }

        if (job.status === 'Completed' || job.status === 'Failed') {
          jobSub.unsubscribe();
        }
      }
    });
  }
}
