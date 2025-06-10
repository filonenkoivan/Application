import { Component, OnInit } from '@angular/core';
import { WorkspaceCardComponent } from '../../features/workspace/components/workspace-card/workspace-card.component';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { WorkspaceResponse } from '../../models/workspace-card-module';

@Component({
  selector: 'app-workspace',
  standalone: true,
  imports: [CommonModule, WorkspaceCardComponent],
  templateUrl: './workspace.component.html',
  styleUrl: './workspace.component.scss',
})
export class WorkspaceComponent implements OnInit {
  workspaces: WorkspaceResponse[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.http
      .get<{ data: WorkspaceResponse[] }>('http://localhost:5086/workspaces')
      .subscribe((response) => {
        this.workspaces = response.data;
      });
  }
}
