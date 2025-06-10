export interface Booking {
  id: number;
  name: string;
  email: string;
  workSpaceType: number;
  startDate: string;
  endDate: string;
  startTime: string;
  endTime: string;
  roomCapacity: number;
  deskNumber: number;
  sessionId: number;
}

export interface BookingResponse {
  startDateTime: string;
  endDateTime: string;
}

export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
}

export interface BookingExistsResponse {
  startDate: string | null;
  endDate: string | null;
  type: string | null;
  exists: boolean;
  capacity: number;
}
