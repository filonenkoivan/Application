import { Component, OnInit } from '@angular/core';
import { WorkspaceCardComponent } from '../../features/workspace/components/workspace-card/workspace-card.component';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { WorkspaceResponse } from '../../models/workspace-card-model';
import { WorkspaceService } from '../../services/workspace-card-service';
import { ActivatedRoute, Route } from '@angular/router';

@Component({
  selector: 'app-workspace',
  standalone: true,
  imports: [CommonModule, WorkspaceCardComponent],
  templateUrl: './workspace.component.html',
  styleUrl: './workspace.component.scss',
})
export class WorkspaceComponent implements OnInit {
  workspaces: WorkspaceResponse[] = [];
  workspaceId: number | null = null;

  constructor(
    private http: HttpClient,
    private workspaceService: WorkspaceService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.workspaceId = Number(this.route.snapshot.queryParamMap.get('id'));

    this.workspaceService
      .getWorkspaces(this.workspaceId)
      .subscribe((response) => {
        this.workspaces = response;
      });
  }
}
