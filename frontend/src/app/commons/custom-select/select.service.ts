import { Injectable } from '@angular/core';
import { CustomSelectComponent } from './custom-select.component';

@Injectable({ providedIn: 'root' })
export class SelectService {
  private currentOpenSelect: CustomSelectComponent | null = null;

  registerOpen(select: CustomSelectComponent) {
    if (this.currentOpenSelect && this.currentOpenSelect !== select) {
      this.currentOpenSelect.close();
    }
    this.currentOpenSelect = select;
  }

  clear(select: CustomSelectComponent) {
    if (this.currentOpenSelect === select) {
      this.currentOpenSelect = null;
    }
  }
}
