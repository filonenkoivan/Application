import { Routes } from '@angular/router';
import { WorkspaceFormComponent } from './features/workspace/components/workspace-form/workspace-form.component';
import { WorkspaceComponent } from './pages/workspace/workspace.component';
import { BookingPageComponent } from './pages/booking-page/booking-page.component';
import { CoworkingPageComponent } from './pages/coworking-page/coworking-page.component';

export const routes: Routes = [
  {
    path: '',
    component: CoworkingPageComponent,
  },
  {
    path: 'workspace',
    component: WorkspaceComponent,
  },
  {
    path: 'booking',
    component: WorkspaceFormComponent,
  },
  {
    path: 'booking/edit/:id',
    component: WorkspaceFormComponent,
  },
  {
    path: 'mybooking',
    component: BookingPageComponent,
  },
];
