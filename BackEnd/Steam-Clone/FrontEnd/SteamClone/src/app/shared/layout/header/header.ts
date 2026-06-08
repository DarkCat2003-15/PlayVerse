import { Component, Input, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../../Core/auth/auth.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [MatButtonModule, MatToolbarModule, RouterLink],
  templateUrl: './header.html',
  styleUrl: './header.scss',
})
export class Header {
  @Input({ required: true }) appName = '';

  readonly authService = inject(AuthService);

  readonly canSeeAdmin = (): boolean => {
    const user = this.authService.currentUser();
    const roles = (user?.roles ?? []).map((role) => role.toLowerCase());
    const permissions = user?.permissions ?? [];

    return roles.includes('admin') || roles.includes('developer') || permissions.includes('GAMES_MANAGE');
  };

  logout(): void {
    this.authService.logout();
  }
}
