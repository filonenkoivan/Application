import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AssistantService } from '../../services/assistant-service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-assistant',
  imports: [FormsModule, CommonModule],
  templateUrl: './assistant.component.html',
  styleUrl: './assistant.component.scss',
})
export class AssistantComponent {
  constructor(private assistantService: AssistantService) {}
  inputValue = '';
  assistResponse = '';

  sendMessageToAssitant() {
    this.assistResponse = 'Looking up your bookings...';
    this.assistantService.sendAssistantRequest(this.inputValue).subscribe({
      next: (el) => {
        this.assistResponse = el.data;
        this.inputValue = '';
      },
      error: (err) => {
        console.log('Error!', err);
      },
    });
  }

  sendExample(event: any) {
    this.assistResponse = 'Looking up your bookings...';
    this.assistantService
      .sendAssistantRequest(event.target.innerText)
      .subscribe({
        next: (el) => {
          this.assistResponse = el.data;
          this.inputValue = '';
        },
        error: (err) => {
          console.log('Error!', err);
        },
      });
  }
}
