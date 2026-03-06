import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

export interface Account {
  id: string;
  accountHolderName: string;
  balance: number;
  accountNumber: string;
  createdAt: string;
}

export interface CreateAccountRequest {
  accountHolderName: string;
}

@Injectable({ providedIn: 'root' })
export class AccountService {
  private readonly apiUrl = `${environment.apiUrl}/account`;
  private http = inject(HttpClient);

  /**
   * GET /api/account
   * Returns all accounts belonging to the logged-in user.
   */
  getMyAccounts() {
    return this.http.get<Account[]>(this.apiUrl);
  }

  /**
   * POST /api/account
   * Creates a new account for the logged-in user.
   */
  createAccount(req: CreateAccountRequest) {
    return this.http.post<Account>(this.apiUrl, req);
  }
}
