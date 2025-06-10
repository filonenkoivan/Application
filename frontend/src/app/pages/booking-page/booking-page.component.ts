import { Component, OnInit } from '@angular/core';
import { BookingComponent } from '../../features/workspace/components/booking/booking.component';
import { Booking } from '../../models/booking-card-model';
import { BookingService } from '../../services/booking-service';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-booking-page',
  imports: [CommonModule, BookingComponent, RouterModule],
  templateUrl: './booking-page.component.html',
  styleUrl: './booking-page.component.scss',
})
export class BookingPageComponent implements OnInit {
  bookings: Booking[] = [];

  constructor(
    private bookingService: BookingService,
    private http: HttpClient
  ) {}

  ngOnInit(): void {
    this.bookingService.getBookings().subscribe((data) => {
      this.bookings = data;
    });
  }

  onDeleteBooking(id: number) {
    this.http.delete(`http://localhost:5086/bookings/${id}`).subscribe({
      next: () => {
        this.bookings = this.bookings.filter((b) => b.id !== id);
      },
      error: (err: any) => {
        console.error('Error deleting booking', err);
      },
    });
  }
}
