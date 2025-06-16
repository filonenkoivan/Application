import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import {
  ApiResponse,
  Booking,
  BookingExistsResponse,
  BookingResponse,
} from '../models/booking-card-model';
import { map, Observable } from 'rxjs';
import { RoomDTO } from '../models/workspace-card-model';

@Injectable({ providedIn: 'root' })
export class BookingService {
  private apiUrl = 'http://localhost:5086/bookings';

  constructor(private http: HttpClient) {}

  getBookings(): Observable<Booking[]> {
    return this.http
      .get<{ data: Booking[] }>(this.apiUrl + '/all', {
        withCredentials: true,
      })
      .pipe(map((response) => response.data));
  }

  getRoomsByType(type: number): Observable<RoomDTO[]> {
    return this.http.get<RoomDTO[]>(this.apiUrl + `/rooms?type=${type - 1}`);
  }

  sendBooking(data: any): Observable<ApiResponse<BookingResponse>> {
    return this.http.post<ApiResponse<BookingResponse>>(this.apiUrl, data);
  }

  getBookingById(id: number): Observable<Booking> {
    return this.http
      .get<{ data: Booking }>(this.apiUrl + `/${id}`)
      .pipe(map((response) => response.data));
  }

  updateBooking(
    id: number,
    payload: any
  ): Observable<ApiResponse<BookingResponse>> {
    return this.http.put<ApiResponse<BookingResponse>>(
      this.apiUrl + `/${id}`,
      payload
    );
  }

  getBookingsByType(
    type: number,
    capacity: number,
    coworkingId: number
  ): Observable<{
    data: BookingResponse[];
    message: string;
    statusCode: number;
  }> {
    return this.http.get<{
      data: BookingResponse[];
      message: string;
      statusCode: number;
    }>(this.apiUrl + `/available`, {
      params: new HttpParams()
        .set('type', type)
        .set('roomCapacity', capacity)
        .set('coworkingId', coworkingId),
    });
  }

  getBookingsByDesk(
    deskId: number,
    coworkingId: number
  ): Observable<{ data: BookingResponse[] }> {
    return this.http.get<{ data: BookingResponse[] }>(
      this.apiUrl + `/desk/${deskId}`,
      {
        params: new HttpParams().set('coworkingId', coworkingId),
      }
    );
  }

  checkBookingExists(
    sessionId: number,
    workspaceId: number,
    cowokringId: number
  ): Observable<BookingExistsResponse> {
    return this.http.get<BookingExistsResponse>(this.apiUrl + `/exists`, {
      params: {
        sessionId: sessionId.toString(),
        workspaceId: workspaceId.toString(),
        coworkingId: cowokringId,
      },
    });
  }
}
