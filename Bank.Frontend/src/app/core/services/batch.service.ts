import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

export interface BatchJob {
  id: string;
  fileName: string;
  totalRecords: number;
  processedRecords: number;
  status: string;
  createdAt: string;
}

export interface BatchUploadResponse {
  message: string;
  jobId: string;
}

@Injectable({ providedIn: 'root' })
export class BatchService {
  private readonly apiUrl = `${environment.apiUrl}/batch`;
  private http = inject(HttpClient);

  /**
   * POST /api/batch/upload (multipart/form-data)
   * Uploads a CSV/JSON file, creates a BatchJob and enqueues a Hangfire job.
   */
  uploadBatch(file: File) {
    const formData = new FormData();
    formData.append('file', file, file.name);
    return this.http.post<BatchUploadResponse>(`${this.apiUrl}/upload`, formData);
  }

  /**
   * GET /api/batch/status/{jobId}
   * Returns status and progress info for a running/completed batch job.
   */
  getBatchStatus(jobId: string) {
    return this.http.get<BatchJob>(`${this.apiUrl}/status/${jobId}`);
  }

  /**
   * GET /api/batch
   * Returns all batch jobs, sorted by most recent.
   */
  getAllBatches() {
    return this.http.get<BatchJob[]>(this.apiUrl);
  }
}
