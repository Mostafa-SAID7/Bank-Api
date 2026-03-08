import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';

export interface UserProfile {
    id: string;
    userName: string;
    email: string;
    firstName: string;
    lastName: string;
}

export interface UpdateProfileRequest {
    firstName: string;
    lastName: string;
}

@Injectable({ providedIn: 'root' })
export class ProfileService {
    private readonly apiUrl = `${environment.apiUrl}/profile`;
    private http = inject(HttpClient);

    /**
     * GET /api/profile — Get current user's profile.
     */
    getProfile(): Observable<UserProfile> {
        return this.http.get<UserProfile>(this.apiUrl);
    }

    /**
     * PUT /api/profile — Update current user's profile.
     */
    updateProfile(req: UpdateProfileRequest): Observable<UserProfile> {
        return this.http.put<UserProfile>(this.apiUrl, req);
    }

    /**
     * DELETE /api/profile — Deactivate current user's account.
     */
    deactivateAccount(): Observable<any> {
        return this.http.delete(this.apiUrl);
    }
}
