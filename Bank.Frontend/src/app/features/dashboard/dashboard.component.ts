import { Component, OnInit, inject } from '@angular/core';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { NgxChartsModule, Color, ScaleType } from '@swimlane/ngx-charts';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { AccountService, Account } from '../../core/services/account.service';
import { TransactionService, Transaction } from '../../core/services/transaction.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    NgxChartsModule,
    MatCardModule,
    MatTableModule,
    MatChipsModule,
    MatProgressSpinnerModule,
    MatIconModule,
    MatButtonModule,
    MatDividerModule,
    CurrencyPipe,
    DatePipe
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  private accountService = inject(AccountService);
  private transactionService = inject(TransactionService);

  accounts: Account[] = [];
  transactions: Transaction[] = [];
  loading = true;
  error = '';

  displayedColumns = ['description', 'type', 'amount', 'status', 'date'];

  get totalBalance() { return this.accounts.reduce((s, a) => s + a.balance, 0); }
  get achCount() { return this.transactions.filter(t => t.type === 'ACH').length; }
  get wpsCount() { return this.transactions.filter(t => t.type === 'WPS').length; }
  get rtgsCount() { return this.transactions.filter(t => t.type === 'RTGS').length; }
  get totalTxns() { return this.transactions.length; }

  get recentTransactions() {
    return [...this.transactions]
      .sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime())
      .slice(0, 10);
  }

  get transactionTypeData() {
    return [
      { name: 'ACH', value: this.achCount },
      { name: 'WPS', value: this.wpsCount },
      { name: 'RTGS', value: this.rtgsCount }
    ].filter(d => d.value > 0);
  }

  colorScheme: Color = {
    name: 'custom',
    selectable: true,
    group: ScaleType.Ordinal,
    domain: ['#6366f1', '#10b981', '#f59e0b']
  };

  ngOnInit() {
    this.accountService.getMyAccounts().subscribe({
      next: (accounts) => {
        this.accounts = accounts;
        if (accounts.length > 0) {
          this.transactionService.getHistory(accounts[0].id).subscribe({
            next: (txns) => { this.transactions = txns; this.loading = false; },
            error: () => { this.loading = false; }
          });
        } else {
          this.loading = false;
        }
      },
      error: () => {
        this.error = 'Failed to load dashboard data from the server.';
        this.loading = false;
      }
    });
  }

  chipColor(status: string): 'primary' | 'accent' | 'warn' {
    if (status === 'Completed') return 'primary';
    if (status === 'Failed') return 'warn';
    return 'accent';
  }
}
