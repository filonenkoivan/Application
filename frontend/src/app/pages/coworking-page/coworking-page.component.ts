import { Component } from '@angular/core';
import { CoworkingDTO } from '../../models/coworking-card-model';
import { CoworkingServices } from '../../services/coworking-service';
import { CoworkingCardComponent } from '../../features/workspace/components/coworking-card/coworking-card.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-coworking-page',
  imports: [CoworkingCardComponent, CommonModule],
  templateUrl: './coworking-page.component.html',
  styleUrl: './coworking-page.component.scss',
})
export class CoworkingPageComponent {
  coworkings: CoworkingDTO[] = [];

  constructor(private coworkingService: CoworkingServices) {}

  ngOnInit(): void {
    this.coworkingService.GetCoworkings().subscribe((data) => {
      this.coworkings = data;
    });
  }
}
