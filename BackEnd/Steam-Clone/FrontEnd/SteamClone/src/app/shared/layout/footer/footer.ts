import { Component, Input } from '@angular/core';
import { APP_SLOGAN, COMPANY_NAME } from '../../../Core/constants/app.constants';

@Component({
  selector: 'app-footer',
  standalone: true,
  templateUrl: './footer.html',
  styleUrl: './footer.scss',
})
export class Footer {
  @Input({ required: true }) appName = '';

  readonly companyName = COMPANY_NAME;
  readonly slogan = APP_SLOGAN;
  readonly currentYear = new Date().getFullYear();
}
