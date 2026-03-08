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

export interface UpdateAccountRequest {
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
   * GET /api/account/{id}
   * Returns a specific account belonging to the logged-in user.
   */
  getAccountById(id: string) {
    return this.http.get<Account>(`${this.apiUrl}/${id}`);
  }

  /**
   * POST /api/account
   * Creates a new account for the logged-in user.
   */
  createAccount(req: CreateAccountRequest) {
    return this.http.post<Account>(this.apiUrl, req);
  }

  /**
   * PUT /api/account/{id}
   * Updates an existing account's holder name.
   */
  updateAccount(id: string, req: UpdateAccountRequest) {
    return this.http.put<void>(`${this.apiUrl}/${id}`, req);
  }

  /**
   * DELETE /api/account/{id}
   * Deletes a specific account.
   */
  deleteAccount(id: string) {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  /**
   * GET /api/admin/accounts
   * Returns all accounts in the system (Admin only).
   */
  getAllAccounts() {
    return this.http.get<Account[]>(`${environment.apiUrl}/admin/accounts`);
  }

  /**
   * GET /api/admin/accounts/{id}
   * Returns a specific account by ID across all users (Admin only).
   */
  getAdminAccountById(id: string) {
    return this.http.get<Account>(`${environment.apiUrl}/admin/accounts/${id}`);
  }
}
