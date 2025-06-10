import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Booking } from '../../../../models/booking-card-model';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDeleteDialogComponent } from '../../../../commons/confirm-delete-dialog/confirm-delete-dialog.component';

@Component({
  selector: 'app-booking',
  imports: [CommonModule],
  templateUrl: './booking.component.html',
  styleUrl: './booking.component.scss',
})
export class BookingComponent {
  @Output() deleteBooking = new EventEmitter<number>();
  @Input() booking!: Booking;
  constructor(private router: Router, private dialog: MatDialog) {}
  getFormattedTime(time: string): string {
    const today = new Date();
    const [hours, minutes, seconds] = time.split(':').map(Number);
    const date = new Date(
      today.getFullYear(),
      today.getMonth(),
      today.getDate(),
      hours,
      minutes,
      seconds
    );
    return date.toLocaleTimeString([], { hour: 'numeric', minute: '2-digit' });
  }

  isSameDay(startDateStr: string, endDateStr: string): boolean {
    const start = new Date(startDateStr);
    const end = new Date(endDateStr);
    return start.toDateString() === end.toDateString();
  }

  getDateDuration(startDateStr: string, endDateStr: string): string {
    const start = new Date(startDateStr);
    const end = new Date(endDateStr);
    const msPerDay = 1000 * 60 * 60 * 24;
    const dayDiff = Math.ceil((end.getTime() - start.getTime()) / msPerDay);
    return `${dayDiff} day${dayDiff > 1 ? 's' : ''}`;
  }

  getTimeDuration(startTime: string, endTime: string): string {
    const [sh, sm] = startTime.split(':').map(Number);
    const [eh, em] = endTime.split(':').map(Number);
    const start = new Date(0, 0, 0, sh, sm);
    const end = new Date(0, 0, 0, eh, em);
    const diffMins = (end.getTime() - start.getTime()) / (1000 * 60);
    const hours = Math.floor(diffMins / 60);
    const minutes = diffMins % 60;

    if (hours && minutes) return `${hours}h ${minutes}m`;
    if (hours) return `${hours} hour${hours > 1 ? 's' : ''}`;
    return `${minutes} minutes`;
  }
  getWorkspaceTypeName(): string {
    switch (this.booking.workSpaceType) {
      case 0:
        return 'Open space';
      case 1:
        return 'Private room';
      case 2:
        return 'Meeting room';
      default:
        return 'Unknown';
    }
  }
  getWorkspaceImage(): string {
    switch (this.booking.workSpaceType) {
      case 0:
        return 'images/workspaces/Openspace.jpg';
      case 1:
        return 'images/workspaces/Privateroom.jpg';
      case 2:
        return 'images/workspaces/Meetingroom.jpg';
      default:
        return 'images/workspaces/Workspace.jpg';
    }
  }

  onDelete() {
    const dialogRef = this.dialog.open(ConfirmDeleteDialogComponent, {
      width: '728px',
      panelClass: 'custom-dialog-container',
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.deleteBooking.emit(this.booking.id);
      }
    });
  }

  editBooking(id: number): void {
    this.router.navigate(['booking/edit/', id]);
  }
}
