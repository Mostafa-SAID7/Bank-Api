import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';

export interface User {
    id: string;
    userName: string;
    email: string;
    firstName: string;
    lastName: string;
}

@Injectable({ providedIn: 'root' })
export class UserService {
    private readonly apiUrl = `${environment.apiUrl}/admin/users`;
    private http = inject(HttpClient);

    /**
     * GET /api/admin/users
     * Returns all users in the system (Admin only).
     */
    getAllUsers(): Observable<User[]> {
        return this.http.get<User[]>(this.apiUrl);
    }

    /**
     * GET /api/admin/users/{id}
     * Returns a specific user in the system (Admin only).
     */
    getUserById(id: string): Observable<User> {
        return this.http.get<User>(`${this.apiUrl}/${id}`);
    }

    /**
     * DELETE /api/admin/users/{id}
     * Suspends (soft-deletes) a user (Admin only).
     */
    suspendUser(id: string): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }
}
