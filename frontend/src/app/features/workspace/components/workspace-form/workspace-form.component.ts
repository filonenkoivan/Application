import { Component, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { BookingService } from '../../../../services/booking-service';
import { DeskDTO, RoomDTO } from '../../../../models/workspace-card-module';
import { ActivatedRoute } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CustomSelectComponent } from '../../../../commons/custom-select/custom-select.component';
import { BookingResponse } from '../../../../models/booking-card-model';
import { CalendarComponent } from '../../../../commons/calendar/calendar.component';
import { CustomDialogComponent } from '../../../../commons/custom-dialog/custom-dialog.component';

@Component({
  standalone: true,
  selector: 'app-workspace-form',
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    CustomSelectComponent,
    CalendarComponent,
    CustomDialogComponent,
  ],
  templateUrl: './workspace-form.component.html',
  styleUrls: ['./workspace-form.component.scss'],
})
export class WorkspaceFormComponent implements OnInit {
  bookingId: number | null = null;
  bookingForm: FormGroup;
  rooms = signal<RoomDTO[]>([]);
  desks = signal<DeskDTO[]>([]);
  desksOptions: { label: string; value: string }[] = [];
  bookedDates: { start: Date; end: Date }[] = [];
  selectionValidFlag = true;

  constructor(
    private fb: FormBuilder,
    private bookingService: BookingService,
    private route: ActivatedRoute
  ) {
    this.bookingForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      workspaceType: ['1'],
      roomSize: ['0', Validators.required],
      desk: ['', Validators.required],
      startDate: [null, Validators.required],
      startTime: ['08:00', Validators.required],
      endDate: [null, Validators.required],
      endTime: ['08:00', Validators.required],
    });

    const idParam = this.route.snapshot.paramMap.get('id');
    const initialType =
      Number(this.bookingForm.get('workspaceType')?.value) || 1;

    if (idParam) {
      const bookingId = Number(idParam);
      this.loadRoomsByType(initialType, () => {
        this.loadBookingData(bookingId);
      });
    } else {
      this.loadRoomsByType(initialType, () => {
        const size = Number(this.bookingForm.get('roomSize')?.value);
        const type = Number(this.bookingForm.get('workspaceType')?.value);
        this.loadBookingRoomsByType(type - 1, size);
      });
    }

    this.bookingForm.get('workspaceType')?.valueChanges.subscribe((typeStr) => {
      const type = Number(typeStr);

      this.loadRoomsByType(type);

      if (type === 2 || type === 3) {
        this.bookingForm.get('roomSize')?.setValidators(Validators.required);
        this.bookingForm.get('desk')?.clearValidators();
        this.bookingForm.patchValue({ desk: '' });
      } else {
        this.bookingForm.get('desk')?.setValidators(Validators.required);
        this.bookingForm.get('roomSize')?.clearValidators();
        this.bookingForm.patchValue({ roomSize: '0' });
      }

      this.bookingForm.get('desk')?.updateValueAndValidity();
      this.bookingForm.get('roomSize')?.updateValueAndValidity();
    });

    this.bookingForm.get('roomSize')?.valueChanges.subscribe((sizeStr) => {
      const size = Number(sizeStr);
      const type = Number(this.bookingForm.get('workspaceType')?.value);
      if (size !== 0) {
        this.loadBookingRoomsByType(type - 1, size);
        this.bookingForm.patchValue({ desk: '' });
      }
      this.bookingForm.patchValue({
        startDateTime: null,
        endDateTime: null,
      });
    });
    this.bookingForm.get('desk')?.valueChanges.subscribe((deskIdStr) => {
      const deskId = Number(deskIdStr);

      if (deskId !== 0) {
        this.loadBookingForDesk(deskId);

        this.bookingForm.patchValue({ roomSize: '0' });
      }
      this.bookingForm.patchValue({
        startDateTime: null,
        endDateTime: null,
      });
    });
  }
  getMaxRangeDays(): number {
    const type = Number(this.bookingForm.get('workspaceType')?.value);

    if (type === 3) {
      return 1;
    }

    return 30;
  }
  loadBookingForDesk(deskId: number) {
    this.bookingService.getBookingsByDesk(deskId).subscribe({
      next: (response) => {
        const bookings = response.data;

        const allSlots: BlockedSlotPerDay[] = [];

        bookings.forEach((booking) => {
          const slots = this.generateBlockedSlotsPerDay(booking);
          allSlots.push(...slots);
        });

        this.blockedSlotsPerDay = allSlots;
      },
      error: (err) => console.error('Error loading desk bookings', err),
    });
  }
  formStartDateTime: Date | null = null;
  formEndDateTime: Date | null = null;

  startDate: Date | null = null;
  endDate: Date | null = null;
  startTime: string = '';
  endTime: string = '';

  ngOnInit(): void {
    this.bookingForm.valueChanges.subscribe((val) => {});
    this.bookingForm.patchValue({
      startDate: this.startDate,
      endDate: this.endDate,
      startTime: this.startTime,
      endTime: this.endTime,
    });
  }
  onCalendarStartDateTimeChange(dateTime: Date | null) {
    this.formStartDateTime = dateTime;

    if (this.formStartDateTime) {
      const formattedTime = this.formatTime(this.formStartDateTime);

      this.bookingForm.patchValue(
        {
          startDate: new Date(this.formStartDateTime),
          startTime: null,
        },
        { emitEvent: false }
      );

      this.bookingForm.patchValue(
        {
          startTime: formattedTime,
        },
        { emitEvent: false }
      );
    } else {
      this.bookingForm.patchValue(
        {
          startDate: null,
          startTime: null,
        },
        { emitEvent: false }
      );
    }
  }

  onCalendarEndDateTimeChange(dateTime: Date | null) {
    this.formEndDateTime = dateTime;

    if (this.formEndDateTime) {
      const formattedTime = this.formatTime(this.formEndDateTime);

      this.bookingForm.patchValue(
        {
          endDate: new Date(this.formEndDateTime),
          endTime: null,
        },
        { emitEvent: false }
      );

      this.bookingForm.patchValue(
        {
          endTime: formattedTime,
        },
        { emitEvent: false }
      );
    } else {
      this.bookingForm.patchValue(
        {
          endDate: null,
          endTime: null,
        },
        { emitEvent: false }
      );
    }
  }

  private updateFormBookingDates() {
    if (this.formStartDateTime) {
      this.bookingForm.patchValue(
        {
          startDate: new Date(this.formStartDateTime),
          startTime: this.formatTime(this.formStartDateTime),
        },
        { emitEvent: false }
      );
    }

    if (this.formEndDateTime) {
      this.bookingForm.patchValue(
        {
          endDate: new Date(this.formEndDateTime),
          endTime: this.formatTime(this.formEndDateTime),
        },
        { emitEvent: false }
      );
    }
  }

  private formatTime(date: Date): string {
    const h = date.getHours().toString().padStart(2, '0');
    const m = date.getMinutes().toString().padStart(2, '0');
    return `${h}:${m}`;
  }

  loadRoomsByType(type: number, callback?: () => void) {
    this.bookingService.getRoomsByType(type).subscribe({
      next: (rooms) => {
        const desks = rooms.slice(0);
        const result = rooms.filter((el) => el.capacity != null);

        if (result.length == 0) {
          this.desks.set(desks);
          this.rooms.set([]);
          this.desksOptions = this.desks().map((el, index) => ({
            label: `Desk ${index + 1}`,
            value: `${el.id}`,
          }));
        } else {
          this.rooms.set(rooms);
          this.desks.set([]);
          this.desksOptions = [];
        }

        if (callback) callback();
      },
      error: (err) => console.error('Error loading rooms', err),
    });
  }

  loadBookingRoomsByType(type: number, capacity: number) {
    this.bookingService.getBookingsByType(type, capacity).subscribe({
      next: (response) => {
        const bookings = response.data;

        const allSlots: BlockedSlotPerDay[] = [];

        bookings.forEach((booking) => {
          const slots = this.generateBlockedSlotsPerDay(booking);
          allSlots.push(...slots);
        });

        this.blockedSlotsPerDay = allSlots;
      },
    });
  }
  getCookie(name: string): string | null {
    const match = document.cookie.match(
      new RegExp('(^| )' + name + '=([^;]+)')
    );
    return match ? match[2] : null;
  }
  onSubmit() {
    if (this.bookingForm.invalid) {
      this.bookingForm.markAllAsTouched();
      return;
    }
    const sessionId = this.getCookie('id');
    const formValue = this.bookingForm.value;

    const startDate = formValue.startDate as Date;
    const endDate = formValue.endDate as Date;

    const [startHour, startMinute] = formValue.startTime.split(':').map(Number);
    const [endHour, endMinute] = formValue.endTime.split(':').map(Number);

    const startDateTime = new Date(startDate);
    startDateTime.setHours(startHour, startMinute, 0, 0);

    const endDateTime = new Date(endDate);
    endDateTime.setHours(endHour, endMinute, 0, 0);

    const startTimeSpan = formValue.startTime + ':00';
    const endTimeSpan = formValue.endTime + ':00';

    const payload = {
      name: formValue.name,
      email: formValue.email,
      workSpaceType: Number(formValue.workspaceType) - 1,
      roomCapacity: Number(formValue.roomSize) || 0,
      deskNumber: Number(formValue.desk) || 0,
      startDate: startDateTime.toISOString(),
      endDate: endDateTime.toISOString(),
      startTime: startTimeSpan,
      endTime: endTimeSpan,
      sessionId: sessionId,
    };

    if (this.bookingId) {
      this.bookingService.updateBooking(this.bookingId, payload).subscribe({
        next: (response) => {
          const message = response?.message;
          this.dialogTitle = "You're all set!";
          this.dialogMessage = `${response.message}`;
          this.dialogLinkUrl = 'mybooking';
          this.dialogLinkText = 'My bookings';
          this.isCorrect = true;
          this.openCustomDialog();
        },
        error: (err) => {
          console.error('Error creating booking:', err);

          let serverMessage = 'Error';

          const message = err.error?.message;
          const data = err.error?.data;

          if (
            message === 'Validation problem' &&
            data &&
            typeof data === 'object'
          ) {
            const allErrors: string[] = [];

            for (const key in data) {
              if (Array.isArray(data[key])) {
                allErrors.push(...data[key]);
              }
            }

            if (allErrors.length > 0) {
              serverMessage = allErrors.join('\n');
            }
          } else if (typeof message === 'string') {
            serverMessage = message;
          }

          this.dialogTitle = 'Error!';
          this.dialogMessage = serverMessage;
          this.dialogLinkUrl = '';
          this.dialogLinkText = 'Close';
          this.isCorrect = false;
          this.openCustomDialog();
        },
      });
    } else {
      this.bookingService.sendBooking(payload).subscribe({
        next: (response) => {
          this.dialogTitle = "You're all set!";
          this.dialogMessage = `${response.message}`;
          this.dialogLinkUrl = 'mybooking';
          this.dialogLinkText = 'My bookings';
          this.isCorrect = true;
          this.openCustomDialog();
        },
        error: (err) => {
          console.error('Error creating booking:', err);

          let serverMessage = 'Error';

          const message = err.error?.message;
          const data = err.error?.data;

          if (
            message === 'Validation problem' &&
            data &&
            typeof data === 'object'
          ) {
            const allErrors: string[] = [];

            for (const key in data) {
              if (Array.isArray(data[key])) {
                allErrors.push(...data[key]);
              }
            }

            if (allErrors.length > 0) {
              serverMessage = allErrors.join('\n');
            }
          } else if (typeof message === 'string') {
            serverMessage = message;
          }
          this.dialogTitle = 'Error!';
          this.dialogMessage = serverMessage;
          this.dialogLinkUrl = '';
          this.dialogLinkText = 'Close';
          this.isCorrect = false;
          this.openCustomDialog();
        },
      });
    }
  }

  workspaceOptions = [
    { label: 'Open space', value: '1' },
    { label: 'Private room', value: '2' },
    { label: 'Meting room', value: '3' },
  ];

  loadBookingData(id: number) {
    this.bookingService.getBookingById(id).subscribe({
      next: (booking) => {
        this.loadRoomsByType(booking.workSpaceType + 1, () => {
          this.bookingForm.patchValue({
            name: booking.name,
            email: booking.email,
            workspaceType: (booking.workSpaceType + 1).toString(),
            roomSize: booking.roomCapacity?.toString() || '0',
            desk: booking.deskNumber.toString(),
          });

          this.loadBookingRoomsByType(
            booking.workSpaceType,
            booking.roomCapacity
          );

          const bookingStartDateOnly = booking.startDate.split('T')[0];
          const bookingEndDateOnly = booking.endDate.split('T')[0];

          const startTime = booking.startTime;
          const endTime = booking.endTime;

          const startDateTime = new Date(
            `${booking.startDate}T${booking.startTime}`
          );
          const endDateTime = new Date(`${booking.endDate}T${booking.endTime}`);

          this.bookingForm.patchValue({
            startDateTime,
            endDateTime,
          });
        });
      },
      error: (err) => {
        console.error('Error loading booking data:', err);
      },
    });
  }

  onStartDateTimeChange(dateTime: Date | null) {
    this.bookingForm.patchValue({ startDateTime: dateTime });
    this.bookingForm.get('startDateTime')?.updateValueAndValidity();
  }

  onEndDateTimeChange(dateTime: Date | null) {
    this.bookingForm.patchValue({ endDateTime: dateTime });
    this.bookingForm.get('endDateTime')?.updateValueAndValidity();
  }

  blockedSlotsPerDay: BlockedSlotPerDay[] = [];

  generateBlockedSlotsPerDay(booking: BookingResponse): BlockedSlotPerDay[] {
    const slots: BlockedSlotPerDay[] = [];

    const startDate = new Date(booking.startDateTime);
    const endDate = new Date(booking.endDateTime);

    const startTime = startDate.toTimeString().slice(0, 5);
    const endTime = endDate.toTimeString().slice(0, 5);

    let currentDate = new Date(startDate);
    currentDate.setHours(0, 0, 0, 0);

    const endDateOnly = new Date(endDate);
    endDateOnly.setHours(0, 0, 0, 0);

    while (currentDate <= endDateOnly) {
      const dateStr = currentDate.toISOString().split('T')[0];

      slots.push({
        date: dateStr,
        startTime,
        endTime,
      });

      currentDate.setDate(currentDate.getDate() + 1);
    }

    return slots;
  }
  getWorkspaceType(): number {
    return Number(this.bookingForm.get('workspaceType')?.value);
  }

  dialogTitle: string = '';
  dialogMessage: string = '';
  dialogLinkUrl: string | null = null;
  dialogLinkText: string = 'Повернутися';
  isCorrect: boolean = true;
  openCustomDialog() {
    const dialogElement = document.getElementById('custom-dialog');
    if (dialogElement) {
      dialogElement.classList.add('open');
    }
  }
}

export interface BlockedSlotPerDay {
  date: string;
  startTime: string;
  endTime: string;
}
