import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AccountService, Account } from '../../core/services/account.service';
import { TransactionService, InitiateTransactionRequest } from '../../core/services/transaction.service';

@Component({
  selector: 'app-payments',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatIconModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatSnackBarModule,
    MatProgressSpinnerModule
  ],
  template: `
    <div class="space-y-6 max-w-5xl mx-auto">
      <!-- Page Header -->
      <div class="flex items-center justify-between">
        <div>
          <h1 class="text-3xl font-bold tracking-tight text-slate-900 dark:text-white">Payments</h1>
          <p class="text-slate-500 dark:text-slate-400 mt-1">Send money quickly and securely.</p>
        </div>
      </div>

      <div class="grid grid-cols-1 lg:grid-cols-3 gap-8">
        <!-- Payment Form -->
        <div class="lg:col-span-2 space-y-6">
          <div class="bg-white dark:bg-slate-900 rounded-3xl border border-slate-200 dark:border-slate-800 p-8 shadow-sm">
            <div class="flex items-center space-x-4 mb-8">
              <div class="w-12 h-12 rounded-2xl bg-indigo-50 dark:bg-indigo-500/10 flex items-center justify-center text-indigo-600 dark:text-indigo-400">
                <mat-icon>send</mat-icon>
              </div>
              <div>
                <h3 class="text-xl font-bold text-slate-900 dark:text-white">Initiate Transfer</h3>
                <p class="text-sm text-slate-500 dark:text-slate-400">Enter details to send funds.</p>
              </div>
            </div>

            <form [formGroup]="paymentForm" (ngSubmit)="onSubmit()" class="space-y-6">
              <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
                <!-- From Account -->
                <mat-form-field appearance="outline" class="w-full">
                  <mat-label>From Account</mat-label>
                  <mat-select formControlName="fromAccountId">
                    <mat-option *ngFor="let acc of accounts" [value]="acc.id">
                      {{ acc.accountNumber }} - {{ acc.balance | currency }}
                    </mat-option>
                  </mat-select>
                  <mat-error *ngIf="paymentForm.get('fromAccountId')?.hasError('required')">Select a source account</mat-error>
                </mat-form-field>

                <!-- Transfer Type -->
                <mat-form-field appearance="outline" class="w-full">
                  <mat-label>Payment Type</mat-label>
                  <mat-select formControlName="type">
                    <mat-option value="ACH">ACH (Standard)</mat-option>
                    <mat-option value="WPS">WPS (Salary)</mat-option>
                    <mat-option value="RTGS">RTGS (Instant)</mat-option>
                  </mat-select>
                </mat-form-field>
              </div>

              <!-- Recipient Account -->
              <mat-form-field appearance="outline" class="w-full">
                <mat-label>Recipient Account Number / ID</mat-label>
                <input matInput formControlName="toAccountId" placeholder="Enter account reference">
                <mat-icon matPrefix class="mr-2 text-slate-400">person</mat-icon>
                <mat-error *ngIf="paymentForm.get('toAccountId')?.hasError('required')">Recipient is required</mat-error>
              </mat-form-field>

              <!-- Amount -->
              <mat-form-field appearance="outline" class="w-full">
                <mat-label>Amount</mat-label>
                <input matInput type="number" formControlName="amount">
                <span matPrefix class="mr-1">$</span>
                <mat-error *ngIf="paymentForm.get('amount')?.hasError('required')">Amount is required</mat-error>
                <mat-error *ngIf="paymentForm.get('amount')?.hasError('min')">Amount must be positive</mat-error>
              </mat-form-field>

              <!-- Description -->
              <mat-form-field appearance="outline" class="w-full">
                <mat-label>Description (Optional)</mat-label>
                <textarea matInput formControlName="description" rows="3"></textarea>
              </mat-form-field>

              <div class="pt-4">
                <button mat-flat-button color="primary" class="!w-full !py-7 !rounded-2xl !text-lg !font-bold" [disabled]="paymentForm.invalid || loading">
                  <span *ngIf="!loading">Send Payment</span>
                  <mat-spinner diameter="24" *ngIf="loading" class="mx-auto"></mat-spinner>
                </button>
              </div>
            </form>
          </div>
        </div>

        <!-- Info Sidebar -->
        <div class="space-y-6">
          <div class="bg-indigo-600 rounded-3xl p-8 text-white shadow-lg shadow-indigo-200 dark:shadow-none">
            <h4 class="text-lg font-bold mb-4">Transfer Guidelines</h4>
            <ul class="space-y-4 text-sm text-indigo-100">
              <li class="flex items-start">
                <mat-icon class="mr-2 !w-5 !h-5 !text-lg">info</mat-icon>
                <span>RTGS transfers are processed instantly within business hours.</span>
              </li>
              <li class="flex items-start">
                <mat-icon class="mr-2 !w-5 !h-5 !text-lg">security</mat-icon>
                <span>All transactions are encrypted and monitored for security.</span>
              </li>
              <li class="flex items-start">
                <mat-icon class="mr-2 !w-5 !h-5 !text-lg">check_circle</mat-icon>
                <span>Double check the recipient account number to avoid delays.</span>
              </li>
            </ul>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    :host {
      display: block;
    }
    ::ng-deep .mat-mdc-form-field-subscript-wrapper {
      display: none;
    }
  `]
})
export class PaymentsComponent implements OnInit {
  private fb = inject(FormBuilder);
  private accountService = inject(AccountService);
  private transactionService = inject(TransactionService);
  private snackBar = inject(MatSnackBar);

  paymentForm: FormGroup;
  accounts: Account[] = [];
  loading = false;

  constructor() {
    this.paymentForm = this.fb.group({
      fromAccountId: ['', Validators.required],
      toAccountId: ['', Validators.required],
      amount: [0, [Validators.required, Validators.min(0.01)]],
      type: ['ACH', Validators.required],
      description: ['']
    });
  }

  ngOnInit() {
    this.loadAccounts();
  }

  loadAccounts() {
    this.accountService.getMyAccounts().subscribe({
      next: (accs) => this.accounts = accs,
      error: () => this.snackBar.open('Failed to load accounts', 'Close', { duration: 3000 })
    });
  }

  onSubmit() {
    if (this.paymentForm.valid) {
      this.loading = true;
      const request: InitiateTransactionRequest = this.paymentForm.value;

      this.transactionService.initiateTransaction(request).subscribe({
        next: (res) => {
          this.loading = false;
          this.snackBar.open('Payment initiated successfully!', 'Success', { duration: 5000 });
          this.paymentForm.reset({ type: 'ACH', amount: 0 });
          this.loadAccounts(); // Refresh balances
        },
        error: (err) => {
          this.loading = false;
          const msg = err.error?.message || 'Payment failed. Please check your balance.';
          this.snackBar.open(msg, 'Close', { duration: 5000 });
        }
      });
    }
  }
}
