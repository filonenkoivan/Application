import { Component, Input } from '@angular/core';
import { CoworkingDTO } from '../../../../models/coworking-card-model';
import { CommonModule, ɵnormalizeQueryParams } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-coworking-card',
  imports: [CommonModule],
  templateUrl: './coworking-card.component.html',
  styleUrl: './coworking-card.component.scss',
})
export class CoworkingCardComponent {
  constructor(private router: Router) {}
  @Input() coworking!: CoworkingDTO;

  goToWorkspace(): void {
    this.router.navigate(['/workspace'], {
      queryParams: {
        id: this.coworking.id,
      },
    });
  }
}
