import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-custom-dialog',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './custom-dialog.component.html',
  styleUrls: ['./custom-dialog.component.scss'],
})
export class CustomDialogComponent {
  @Input() title: string = '';
  @Input() message: string = '';
  @Input() linkUrl: string | null = null;
  @Input() linkText: string = 'Повернутися';
  @Input() isCorrect: boolean = true;

  closeDialog() {
    const dialogElement = document.getElementById('custom-dialog');
    if (dialogElement) {
      dialogElement.classList.remove('open');
    }
  }

  openDialog() {
    const dialogElement = document.getElementById('custom-dialog');
    if (dialogElement) {
      dialogElement.classList.add('open');
    }
    console.log(this.isCorrect);
  }
}
