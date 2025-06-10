import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import {
  ApiResponse,
  Booking,
  BookingExistsResponse,
  BookingResponse,
} from '../models/booking-card-model';
import { map, Observable } from 'rxjs';
import { RoomDTO } from '../models/workspace-card-module';

@Injectable({ providedIn: 'root' })
export class BookingService {
  private apiUrl = 'http://localhost:5086/bookings';

  constructor(private http: HttpClient) {}

  getBookings(): Observable<Booking[]> {
    return this.http
      .get<{ data: Booking[] }>(this.apiUrl)
      .pipe(map((response) => response.data));
  }

  getRoomsByType(type: number): Observable<RoomDTO[]> {
    return this.http.get<RoomDTO[]>(
      `http://localhost:5086/bookings/rooms?type=${type - 1}`
    );
  }

  sendBooking(data: any): Observable<ApiResponse<BookingResponse>> {
    return this.http.post<ApiResponse<BookingResponse>>(
      'http://localhost:5086/bookings',
      data
    );
  }

  getBookingById(id: number): Observable<Booking> {
    return this.http
      .get<{ data: Booking }>(`http://localhost:5086/bookings/${id}`)
      .pipe(map((response) => response.data));
  }

  updateBooking(
    id: number,
    payload: any
  ): Observable<ApiResponse<BookingResponse>> {
    return this.http.put<ApiResponse<BookingResponse>>(
      `http://localhost:5086/bookings/${id}`,
      payload
    );
  }

  getBookingsByType(
    type: number,
    capacity: number
  ): Observable<{
    data: BookingResponse[];
    message: string;
    statusCode: number;
  }> {
    return this.http.get<{
      data: BookingResponse[];
      message: string;
      statusCode: number;
    }>(`http://localhost:5086/bookings/available`, {
      params: new HttpParams().set('type', type).set('roomCapacity', capacity),
    });
  }

  getBookingsByDesk(deskId: number): Observable<{ data: BookingResponse[] }> {
    return this.http.get<{ data: BookingResponse[] }>(
      `http://localhost:5086/bookings/desk/${deskId}`
    );
  }

  checkBookingExists(
    sessionId: number,
    workspaceId: number
  ): Observable<BookingExistsResponse> {
    return this.http.get<BookingExistsResponse>(
      `http://localhost:5086/bookings/exists`,
      {
        params: {
          sessionId: sessionId.toString(),
          workspaceId: workspaceId.toString(),
        },
      }
    );
  }
}
