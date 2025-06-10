import { Routes } from '@angular/router';
import { WorkspaceFormComponent } from './features/workspace/components/workspace-form/workspace-form.component';
import { WorkspaceCardComponent } from './features/workspace/components/workspace-card/workspace-card.component';
import { BookingComponent } from './features/workspace/components/booking/booking.component';
import { WorkspaceComponent } from './pages/workspace/workspace.component';
import { BookingPageComponent } from './pages/booking-page/booking-page.component';

export const routes: Routes = [
  {
    path: '',
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
