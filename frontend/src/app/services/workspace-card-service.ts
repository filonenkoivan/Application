import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { WorkspaceResponse } from '../models/workspace-card-module';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class WorkspaceService {
  private apiUrl = 'http://localhost:5086';

  constructor(private http: HttpClient) {}

  getWorkspaces(): Observable<WorkspaceResponse[]> {
    return this.http
      .get<{ data: WorkspaceResponse[] }>(this.apiUrl)
      .pipe(map((response) => response.data));
  }
}
