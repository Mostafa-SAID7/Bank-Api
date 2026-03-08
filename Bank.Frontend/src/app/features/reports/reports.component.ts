import { Component, OnInit, inject } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { NgxChartsModule, Color, ScaleType, LegendPosition } from '@swimlane/ngx-charts';
import { AccountService, Account } from '../../core/services/account.service';
import { TransactionService, Transaction } from '../../core/services/transaction.service';
import { forkJoin, of } from 'rxjs';
import { switchMap, catchError } from 'rxjs/operators';

@Component({
  selector: 'app-reports',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatButtonModule, MatSelectModule, MatFormFieldModule, MatProgressSpinnerModule, NgxChartsModule, CurrencyPipe],
  template: `
    <div class="space-y-6">
      <!-- Page Header -->
      <div class="flex items-center justify-between">
        <div>
          <h1 class="text-3xl font-bold tracking-tight text-slate-900 dark:text-white">Reports & Analytics</h1>
          <p class="text-slate-500 dark:text-slate-400 mt-1">Real-time financial insights powered by your data.</p>
        </div>
        <div class="flex items-center space-x-3">
          <button mat-stroked-button class="!rounded-xl px-4 py-6 !border-slate-200 dark:!border-slate-800">
            <mat-icon class="mr-2">file_download</mat-icon>
            Export PDF
          </button>
          <button mat-flat-button color="primary" class="!rounded-xl px-4 py-6" (click)="loadData()">
            <mat-icon class="mr-2">refresh</mat-icon>
            Refresh
          </button>
        </div>
      </div>

      <div *ngIf="loading" class="flex justify-center py-16">
        <mat-spinner diameter="40"></mat-spinner>
      </div>

      <div *ngIf="!loading">
        <!-- Quick Stats -->
        <div class="grid grid-cols-1 md:grid-cols-4 gap-6 mb-6">
          <div class="bg-white dark:bg-slate-900 rounded-3xl border border-slate-200 dark:border-slate-800 p-6 shadow-sm">
            <div class="flex items-center justify-between mb-4">
              <div class="w-10 h-10 rounded-2xl flex items-center justify-center bg-emerald-50 dark:bg-emerald-500/10">
                <mat-icon class="text-emerald-600 dark:text-emerald-400">account_balance</mat-icon>
              </div>
            </div>
            <div class="text-2xl font-bold text-slate-900 dark:text-white">{{ totalBalance | currency }}</div>
            <div class="text-sm text-slate-500 dark:text-slate-400 mt-1">Total Balance</div>
          </div>

          <div class="bg-white dark:bg-slate-900 rounded-3xl border border-slate-200 dark:border-slate-800 p-6 shadow-sm">
            <div class="flex items-center justify-between mb-4">
              <div class="w-10 h-10 rounded-2xl flex items-center justify-center bg-indigo-50 dark:bg-indigo-500/10">
                <mat-icon class="text-indigo-600 dark:text-indigo-400">swap_horiz</mat-icon>
              </div>
            </div>
            <div class="text-2xl font-bold text-slate-900 dark:text-white">{{ totalTransactions }}</div>
            <div class="text-sm text-slate-500 dark:text-slate-400 mt-1">Total Transactions</div>
          </div>

          <div class="bg-white dark:bg-slate-900 rounded-3xl border border-slate-200 dark:border-slate-800 p-6 shadow-sm">
            <div class="flex items-center justify-between mb-4">
              <div class="w-10 h-10 rounded-2xl flex items-center justify-center bg-amber-50 dark:bg-amber-500/10">
                <mat-icon class="text-amber-600 dark:text-amber-400">credit_card</mat-icon>
              </div>
            </div>
            <div class="text-2xl font-bold text-slate-900 dark:text-white">{{ accounts.length }}</div>
            <div class="text-sm text-slate-500 dark:text-slate-400 mt-1">Active Accounts</div>
          </div>

          <div class="bg-white dark:bg-slate-900 rounded-3xl border border-slate-200 dark:border-slate-800 p-6 shadow-sm">
            <div class="flex items-center justify-between mb-4">
              <div class="w-10 h-10 rounded-2xl flex items-center justify-center bg-rose-50 dark:bg-rose-500/10">
                <mat-icon class="text-rose-600 dark:text-rose-400">trending_up</mat-icon>
              </div>
            </div>
            <div class="text-2xl font-bold text-slate-900 dark:text-white">{{ completedRate | number:'1.0-1' }}%</div>
            <div class="text-sm text-slate-500 dark:text-slate-400 mt-1">Success Rate</div>
          </div>
        </div>

        <!-- Charts -->
        <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
          <div class="bg-white dark:bg-slate-900 rounded-3xl border border-slate-200 dark:border-slate-800 p-8 shadow-sm">
            <h3 class="text-lg font-bold text-slate-900 dark:text-white mb-6">Transaction Types</h3>
            <div class="h-64" *ngIf="typeChartData.length > 0">
              <ngx-charts-pie-chart
                [results]="typeChartData"
                [scheme]="colorScheme"
                [legend]="true"
                [legendPosition]="LegendPosition.Below"
                [doughnut]="true">
              </ngx-charts-pie-chart>
            </div>
            <div class="h-64 rounded-2xl bg-slate-50 dark:bg-slate-800/50 flex items-center justify-center border-2 border-dashed border-slate-200 dark:border-slate-800" *ngIf="typeChartData.length === 0">
              <p class="text-slate-400 dark:text-slate-500 italic">No transaction data available.</p>
            </div>
          </div>

          <div class="bg-white dark:bg-slate-900 rounded-3xl border border-slate-200 dark:border-slate-800 p-8 shadow-sm">
            <h3 class="text-lg font-bold text-slate-900 dark:text-white mb-6">Transaction Status</h3>
            <div class="h-64" *ngIf="statusChartData.length > 0">
              <ngx-charts-bar-vertical
                [results]="statusChartData"
                [scheme]="colorScheme"
                [xAxisLabel]="'Status'"
                [yAxisLabel]="'Count'"
                [showXAxisLabel]="true"
                [showYAxisLabel]="true"
                [xAxis]="true"
                [yAxis]="true"
                [roundDomains]="true">
              </ngx-charts-bar-vertical>
            </div>
            <div class="h-64 rounded-2xl bg-slate-50 dark:bg-slate-800/50 flex items-center justify-center border-2 border-dashed border-slate-200 dark:border-slate-800" *ngIf="statusChartData.length === 0">
              <p class="text-slate-400 dark:text-slate-500 italic">No transaction data available.</p>
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
export class ReportsComponent implements OnInit {
  private accountService = inject(AccountService);
  private transactionService = inject(TransactionService);

  accounts: Account[] = [];
  transactions: Transaction[] = [];
  loading = true;

  colorScheme: Color = {
    name: 'custom',
    selectable: true,
    group: ScaleType.Ordinal,
    domain: ['#6366f1', '#10b981', '#f59e0b', '#ef4444']
  };

  LegendPosition = LegendPosition;

  get totalBalance() {
    return this.accounts.reduce((s, a) => s + a.balance, 0);
  }

  get totalTransactions() {
    return this.transactions.length;
  }

  get completedRate() {
    if (this.transactions.length === 0) return 0;
    const completed = this.transactions.filter(t => t.status === 'Completed').length;
    return (completed / this.transactions.length) * 100;
  }

  get typeChartData() {
    const ach = this.transactions.filter(t => t.type === 'ACH').length;
    const wps = this.transactions.filter(t => t.type === 'WPS').length;
    const rtgs = this.transactions.filter(t => t.type === 'RTGS').length;
    return [
      { name: 'ACH', value: ach },
      { name: 'WPS', value: wps },
      { name: 'RTGS', value: rtgs }
    ].filter(d => d.value > 0);
  }

  get statusChartData() {
    const completed = this.transactions.filter(t => t.status === 'Completed').length;
    const pending = this.transactions.filter(t => t.status === 'Pending').length;
    const failed = this.transactions.filter(t => t.status === 'Failed').length;
    return [
      { name: 'Completed', value: completed },
      { name: 'Pending', value: pending },
      { name: 'Failed', value: failed }
    ].filter(d => d.value > 0);
  }

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.loading = true;
    this.accountService.getMyAccounts().pipe(
      switchMap(accounts => {
        this.accounts = accounts;
        if (accounts.length > 0) {
          const historyObservables = accounts.map(acc =>
            this.transactionService.getHistory(acc.id).pipe(catchError(() => of([] as Transaction[])))
          );
          return forkJoin(historyObservables);
        }
        return of([]);
      }),
      catchError(() => {
        this.loading = false;
        return of([]);
      })
    ).subscribe(results => {
      this.transactions = (results as Transaction[][]).flat();
      this.loading = false;
    });
  }
}
