import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
  SimpleChanges,
} from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { provideNativeDateAdapter } from '@angular/material/core';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule } from '@angular/forms';
import { NgxMaterialTimepickerModule } from 'ngx-material-timepicker';
import { CommonModule } from '@angular/common';
import { MatSelectModule } from '@angular/material/select';
import { BlockedSlotPerDay } from '../../features/workspace/components/workspace-form/workspace-form.component';
import { routes } from '../../app.routes';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-calendar',
  imports: [
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
    FormsModule,
    NgxMaterialTimepickerModule,
    CommonModule,
    MatSelectModule,
  ],
  templateUrl: './calendar.component.html',
  styleUrl: './calendar.component.scss',
  providers: [provideNativeDateAdapter()],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CalendarComponent {
  @Input() defaultStartTimeValue: string = '';
  @Input() defaultEndTimeValue: string = '';

  @Input() startDateTime: Date | null = null;
  @Input() endDateTime: Date | null = null;
  internalStartDate: Date | null = null;
  internalEndDate: Date | null = null;

  startTime: string | null = '08:00';
  endTime: string | null = '08:00';

  isStartUpdated: boolean = false;
  isEndUpdated: boolean = false;

  constructor(private router: ActivatedRoute) {
    this.generateTimeOptions();
  }
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['startDateTime']) {
      if (this.isStartUpdated) {
        this.defaultStartTimeValue = '';
        this.isStartUpdated = false;
      }

      this.internalStartDate = this.startDateTime;

      if (this.startDateTime) {
        this.startDate = new Date(this.startDateTime);
        this.startTime = this.defaultStartTimeValue || this.startTime || '';
        if (this.defaultStartTimeValue) {
          this.isStartUpdated = true;
        }
      } else {
        this.startDate = null;
        this.startTime = this.defaultStartTimeValue
          ? this.defaultStartTimeValue
          : '08:00';
      }
    }

    if (changes['endDateTime']) {
      if (this.isEndUpdated) {
        this.defaultEndTimeValue = '';
        this.isEndUpdated = false;
      }
      this.internalEndDate = this.endDateTime;

      if (this.endDateTime) {
        this.endDate = new Date(this.endDateTime);
        this.endTime = this.defaultEndTimeValue || this.endTime || '';
        if (this.defaultEndTimeValue) {
          this.isEndUpdated = true;
        }
      } else {
        this.endDate = null;
        this.endTime = this.defaultEndTimeValue
          ? this.defaultEndTimeValue
          : '08:00';
      }
    }
  }

  private _workspaceType: string = '1';

  @Input()
  set workspaceType(value: string) {
    this._workspaceType = value;
    this.startDate = null;
    this.endDate = null;

    this.startTime = null;
    this.endTime = null;

    setTimeout(() => {
      this.startTime = '08:00';
      this.endTime = '08:00';

      this.emitStartDateTime();
      this.emitEndDateTime();
    });
  }

  get workspaceType(): string {
    return this._workspaceType;
  }

  get minSelectableDate(): Date | null {
    if (this.workspaceType === '3') {
      return null;
    } else {
      return new Date();
    }
  }
  @Input() blockedSlotsPerDay: BlockedSlotPerDay[] = [];

  @Input() bookedDates: { start: Date; end: Date }[] = [];

  isSlotBlocked(selectedDate: Date, selectedTime: string): boolean {
    const [hours, minutes] = selectedTime.split(':').map(Number);
    const dateTime = new Date(selectedDate);
    dateTime.setHours(hours, minutes, 0, 0);

    return this.bookedDates.some((slot) => {
      return dateTime >= slot.start && dateTime < slot.end;
    });
  }
  @Input() maxRangeDays: number = 30;
  @Input() startDate: Date | null = null;
  @Input() endDate: Date | null = null;

  @Output() startDateTimeChange = new EventEmitter<Date | null>();
  @Output() endDateTimeChange = new EventEmitter<Date | null>();

  onDateSelected(date: Date) {
    if (!this.startDate || (this.startDate && this.endDate)) {
      this.startDate = date;
      this.endDate = null;

      if (!this.startTime) {
        this.startTime = '08:00';
      }

      this.emitStartDateTime();
      this.endDateTimeChange.emit(null);
    } else {
      if (date >= this.startDate) {
        this.endDate = date;
        if (!this.endTime) {
          this.endTime = '08:00';
        }

        this.emitEndDateTime();
      } else {
        this.startDate = date;
        this.endDate = null;

        if (!this.startTime) {
          this.startTime = '08:00';
        }

        this.emitStartDateTime();
        this.endDateTimeChange.emit(null);
      }
    }
  }

  timeOptions: string[] = [];

  generateTimeOptions() {
    const startHour = 8;
    const endHour = 20;
    const stepMinutes = 30;

    for (let hour = startHour; hour <= endHour; hour++) {
      for (let min = 0; min < 60; min += stepMinutes) {
        const h = hour < 10 ? '0' + hour : hour.toString();
        const m = min < 10 ? '0' + min : min.toString();
        this.timeOptions.push(`${h}:${m}`);
      }
    }
  }
  isTimeBlocked(date: Date, time: string): boolean {
    const dateStr =
      date.getFullYear() +
      '-' +
      String(date.getMonth() + 1).padStart(2, '0') +
      '-' +
      String(date.getDate()).padStart(2, '0');

    const slotsForDate = this.blockedSlotsPerDay.filter(
      (slot) => slot.date === dateStr
    );

    for (const slot of slotsForDate) {
      if (time >= slot.startTime && time < slot.endTime) {
        return true;
      }
    }

    return false;
  }
  onStartDateSelected(date: Date | null) {
    if (!date) return;

    this.startDate = date;

    this.startTime = '08:00';

    this.emitStartDateTime();

    if (this.workspaceType === '3') {
      this.endDate = date;
      if (!this.endTime) {
        this.endTime = '08:00';
      }
      this.emitEndDateTime();
    }
  }
  onStartTimeChanged(time: string) {
    this.startTime = time;
    this.emitStartDateTime();
  }
  private emitStartDateTime() {
    const date = this.startDate ?? new Date();

    if (this.startTime) {
      const [hour, minute] = this.startTime.split(':').map(Number);
      const dt = new Date(date);
      dt.setHours(hour, minute, 0, 0);
      console.log(this.startTime);
      this.startDateTimeChange.emit(dt);
    } else {
      this.startDateTimeChange.emit(null);
    }
  }

  onEndDateSelected(date: Date | null) {
    if (!date) {
      this.endDate = null;
      this.endDateTimeChange.emit(null);
      return;
    }
    this.endDate = date;

    if (!this.endTime) {
      this.endTime = '08:00';
    }

    this.emitEndDateTime();
  }

  onEndTimeChanged(time: string) {
    this.endTime = time;
    this.emitEndDateTime();
  }

  private emitEndDateTime() {
    const date = this.endDate ?? new Date();

    if (this.endTime) {
      const [hour, minute] = this.endTime.split(':').map(Number);
      const dt = new Date(date);
      dt.setHours(hour, minute, 0, 0);
      this.endDateTimeChange.emit(dt);
    } else {
      this.endDateTimeChange.emit(null);
    }
  }

  get maxEndDate(): Date | null {
    if (this.workspaceType === '3') {
      return null;
    }

    if (!this.startDate) {
      return null;
    }

    const maxDate = new Date(this.startDate);

    switch (this.workspaceType) {
      case '1':
        maxDate.setDate(maxDate.getDate() + 30);
        return maxDate;

      case '2':
        maxDate.setDate(maxDate.getDate() + 30);
        return maxDate;

      default:
        return null;
    }
  }
  minDate: Date = new Date();
}

// private emitEndDateTime() {
//   if (!this.endDate) {
//     this.endDateTimeChange.emit(null);
//     return;
//   }

//   const date = new Date(this.endDate);

//   if (this.endTime) {
//     const [hour, minute] = this.endTime.split(':').map(Number);
//     date.setHours(hour, minute, 0, 0);
//   } else {
//     date.setHours(8, 0, 0, 0); // дефолтний час
//   }

//   this.endDateTimeChange.emit(date);
// }
