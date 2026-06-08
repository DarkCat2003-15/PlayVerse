import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../Core/auth/auth.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [MatButtonModule, RouterLink, RouterLinkActive],
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss',
})
export class Navbar {
  readonly authService = inject(AuthService);

  readonly canSeeAdmin = (): boolean => {
    const user = this.authService.currentUser();
    const roles = (user?.roles ?? []).map((role) => role.toLowerCase());
    const permissions = user?.permissions ?? [];

    return roles.includes('admin') || roles.includes('developer') || permissions.includes('GAMES_MANAGE');
  };
}
