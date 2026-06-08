import { Component, EventEmitter, Input, Output } from '@angular/core';

export interface ConfirmDialogData {
  title: string;
  message: string;
  cancelText?: string;
  confirmText?: string;
}

@Component({
  selector: 'app-confirm-dialog',
  standalone: true,
  templateUrl: './confirm-dialog.html',
  styleUrl: './confirm-dialog.scss',
})
export class ConfirmDialog {
  @Input() data: ConfirmDialogData = {
    title: 'Confirmar accion',
    message: 'Estas seguro de continuar?',
  };

  @Output() readonly closed = new EventEmitter<boolean>();

  close(result: boolean): void {
    this.closed.emit(result);
  }
}
