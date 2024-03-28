import { Injectable } from '@angular/core';
import { MessageService } from 'primeng/api';

@Injectable({
  providedIn: 'root',
})
export class CustomMessageService {
  constructor(private messageService: MessageService) {}

  showSuccessMessage(message: string): void {
    this.messageService.add({
      severity: 'success',
      summary: 'Success',
      detail: message,
    });
  }

  showErrorMessage(message: string): void {
    this.messageService.add({
      severity: 'error',
      summary: 'Error',
      detail: message,
    });
  }
  
  showRejectedMessage(message: string): void {
    this.messageService.add({
      severity: 'error',
      summary: 'Rejected',
      detail: message,
      life: 3000,
    });
  }
}
