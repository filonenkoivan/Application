import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { WorkspaceResponse } from '../models/workspace-card-model';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class WorkspaceService {
  private apiUrl = 'http://localhost:5086/workspaces';

  constructor(private http: HttpClient) {}

  getWorkspaces(id: number): Observable<WorkspaceResponse[]> {
    return this.http
      .get<{ data: WorkspaceResponse[] }>(`${this.apiUrl}/${id}`)
      .pipe(map((response) => response.data));
  }
}
