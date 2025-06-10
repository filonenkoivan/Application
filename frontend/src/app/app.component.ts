import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './layout/header/header.component';
import { CommonModule, Location } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, HeaderComponent, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  title = 'co-working';
  constructor(public router: Router, private location: Location) {}
  goBack(): void {
    this.location.back();
  }

  ngOnInit(): void {
    if (!document.cookie.includes('id=')) {
      const randomId = Math.floor(Math.random() * 100).toString();
      document.cookie = `id=${randomId}; path=/`;
    }
  }
}
