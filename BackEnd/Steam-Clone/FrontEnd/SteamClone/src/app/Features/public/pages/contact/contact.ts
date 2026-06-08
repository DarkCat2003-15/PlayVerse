import { Component, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { CONTACT_EMAIL, CONTACT_PHONE } from '../../../../Core/constants/app.constants';

@Component({
  selector: 'app-contact',
  standalone: true,
  imports: [MatButtonModule, MatCardModule, MatFormFieldModule, MatInputModule],
  templateUrl: './contact.html',
  styleUrl: './contact.scss',
})
export class Contact {
  readonly email = CONTACT_EMAIL;
  readonly phone = CONTACT_PHONE;
  readonly formSent = signal(false);

  submitContact(event: Event): void {
    event.preventDefault();
    this.formSent.set(true);
  }
}
