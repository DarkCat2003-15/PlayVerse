import { Component } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { COMPANY_NAME } from '../../../../Core/constants/app.constants';

@Component({
  selector: 'app-about',
  standalone: true,
  imports: [MatCardModule],
  templateUrl: './about.html',
  styleUrl: './about.scss',
})
export class About {
  readonly companyName = COMPANY_NAME;

  readonly teamMembers = [
    {
      name: 'Integrante 1',
      role: 'Backend y Base de datos',
      description: 'Se encargo de la API REST, autenticacion, reglas de negocio y estructura SQL Server.',
    },
    {
      name: 'Integrante 2',
      role: 'Frontend',
      description: 'Se encargo de la interfaz publica, panel privado, consumo de API y experiencia visual.',
    },
  ];
}
