import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { WorkspaceCardComponent } from './common-ui/workspace-card/workspace-card.component';
import { HeaderComponent } from './common-ui/header/header.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, WorkspaceCardComponent, HeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  title = 'co-working';
}
