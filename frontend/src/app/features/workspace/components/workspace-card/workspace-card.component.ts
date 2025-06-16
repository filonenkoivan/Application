import {
  AfterViewInit,
  Component,
  CUSTOM_ELEMENTS_SCHEMA,
  ElementRef,
  Input,
  OnInit,
  ViewChild,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { WorkspaceResponse } from '../../../../models/workspace-card-model';
import { CommonModule } from '@angular/common';
import { BookingExistsResponse } from '../../../../models/booking-card-model';
import { BookingService } from '../../../../services/booking-service';

@Component({
  selector: 'app-workspace-card',
  imports: [CommonModule],
  templateUrl: './workspace-card.component.html',
  styleUrl: './workspace-card.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class WorkspaceCardComponent implements OnInit, AfterViewInit {
  @ViewChild('mainSwiper') mainSwiper!: ElementRef;
  @ViewChild('thumbSwiper') thumbSwiper!: ElementRef;
  ngAfterViewInit() {
    setTimeout(() => {
      if (this.mainSwiper?.nativeElement && this.thumbSwiper?.nativeElement) {
        const mainSwiperInstance = this.mainSwiper.nativeElement.swiper;
        const thumbSwiperInstance = this.thumbSwiper.nativeElement.swiper;

        if (mainSwiperInstance && thumbSwiperInstance) {
          if (mainSwiperInstance.thumbs) {
            mainSwiperInstance.thumbs.swiper = thumbSwiperInstance;
            mainSwiperInstance.thumbs.init();
          }
        }
      }
    }, 100);
  }

  componentId = Math.random().toString(36).substr(2, 9);
  @Input() workspace!: WorkspaceResponse;

  groupedRooms: { capacity: number; count: number }[] = [];

  bookingExistsResponse: BookingExistsResponse | null = null;

  constructor(
    private router: Router,
    private bookingService: BookingService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.groupRooms();
    this.checkBookingExists();
  }

  private groupRooms(): void {
    const map = new Map<number, number>();

    for (const room of this.workspace.availabilityRooms || []) {
      map.set(room.capacity, (map.get(room.capacity) || 0) + 1);
    }

    this.groupedRooms = Array.from(map.entries()).map(([capacity, count]) => ({
      capacity,
      count,
    }));
  }

  private checkBookingExists(): void {
    const sessionId = this.getSessionIdFromCookie();
    const coworkingId = this.route.snapshot.queryParamMap.get('id');
    if (!sessionId) {
      console.warn('Session ID not found in cookies');
      return;
    }

    this.bookingService
      .checkBookingExists(
        Number(sessionId),
        this.workspace.workSpaceType,
        Number(coworkingId)
      )
      .subscribe({
        next: (response: BookingExistsResponse) => {
          this.bookingExistsResponse = response;
        },
        error: (error: any) => {
          console.error('Error checking booking exists:', error);
        },
      });
  }

  private getSessionIdFromCookie(): string | null {
    const name = 'id=';
    const decodedCookie = decodeURIComponent(document.cookie);
    const ca = decodedCookie.split(';');

    for (let c of ca) {
      c = c.trim();
      if (c.indexOf(name) === 0) {
        return c.substring(name.length);
      }
    }

    return null;
  }

  goToBooking(): void {
    this.router.navigate(['/booking'], {
      queryParams: {
        workspaceId: this.workspace.id,
        type: this.workspace.workSpaceType,
        coworkingId: Number(this.route.snapshot.queryParamMap.get('id')),
      },
    });
  }
}
