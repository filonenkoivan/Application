import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/booking-card-model';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class AssistantService {
  private apiUrl = 'http://localhost:5086/assistant';
  constructor(private http: HttpClient) {}

  sendAssistantRequest(message: string): Observable<ApiResponse<string>> {
    return this.http.post<ApiResponse<string>>(
      this.apiUrl,
      { message },
      { withCredentials: true }
    );
  }
}
