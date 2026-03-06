import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

export type TransactionType = 'ACH' | 'WPS' | 'RTGS';

export interface Transaction {
  id: string;
  fromAccountId: string;
  toAccountId: string;
  amount: number;
  type: TransactionType;
  description: string;
  status: string;
  createdAt: string;
}

export interface InitiateTransactionRequest {
  fromAccountId: string;
  toAccountId: string;
  amount: number;
  type: TransactionType;
  description: string;
}

@Injectable({ providedIn: 'root' })
export class TransactionService {
  private readonly apiUrl = `${environment.apiUrl}/transaction`;
  private http = inject(HttpClient);

  /**
   * POST /api/transaction
   * Initiates a new payment transaction via MediatR command.
   */
  initiateTransaction(req: InitiateTransactionRequest) {
    return this.http.post<Transaction>(this.apiUrl, req);
  }

  /**
   * GET /api/transaction/history/{accountId}
   * Returns paginated transaction history for an account.
   */
  getHistory(accountId: string) {
    return this.http.get<Transaction[]>(`${this.apiUrl}/history/${accountId}`);
  }
}
