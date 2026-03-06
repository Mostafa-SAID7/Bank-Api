import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { BehaviorSubject, tap, map } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface LoginRequest { email: string; password: string; }
export interface RegisterRequest { username: string; email: string; password: string; }
export interface AuthResponse { token: string; }

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly apiUrl = `${environment.apiUrl}/auth`;
  private readonly TOKEN_KEY = 'finbank_jwt_token';

  private http = inject(HttpClient);
  private router = inject(Router);

  private _isLoggedIn$ = new BehaviorSubject<boolean>(!!this.getToken());
  isLoggedIn$ = this._isLoggedIn$.asObservable();

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  login(payload: LoginRequest) {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, payload).pipe(
      tap(res => {
        localStorage.setItem(this.TOKEN_KEY, res.token);
        this._isLoggedIn$.next(true);
      })
    );
  }

  register(payload: RegisterRequest) {
    return this.http.post<any>(`${this.apiUrl}/register`, payload);
  }

  logout() {
    localStorage.removeItem(this.TOKEN_KEY);
    this._isLoggedIn$.next(false);
    this.router.navigate(['/login']);
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }
}
