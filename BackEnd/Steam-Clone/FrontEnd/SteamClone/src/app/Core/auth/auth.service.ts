import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { map, Observable, tap } from 'rxjs';
import { environment } from '../../../environment/environment';
import {
  AUTH_REFRESH_TOKEN_KEY,
  AUTH_TOKEN_KEY,
  AUTH_USER_KEY,
} from '../constants/app.constants';
import { ApiResponse } from '../api/models/api.interface';
import {
  AuthUser,
  LoginRequest,
  LoginResponse,
  RefreshTokenResponse,
  SignUpRequest,
  UpdateAccountRequest,
} from './models/auth.interface';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);
  private readonly loginUrl = `${environment.apiUrl}/auth/login`;
  private readonly meUrl = `${environment.apiUrl}/user/me`;
  private readonly userUrl = `${environment.apiUrl}/user`;
  private readonly refreshUrl = `${environment.apiUrl}/auth/renew`;

  readonly currentUser = signal<AuthUser | null>(this.loadUserFromStorage());

  login(credentials: LoginRequest): Observable<AuthUser> {
    return this.http.post<ApiResponse<LoginResponse>>(this.loginUrl, credentials).pipe(
      map((response) => response.data),
      tap((response) => {
        this.persistSession(response);
      }),
      map((response) => this.toAuthUser(response)),
    );
  }

  getCurrentUser(): Observable<AuthUser> {
    return this.http.get<ApiResponse<AuthUser>>(this.meUrl).pipe(
      map((response) => response.data),
      tap((user) => {
        const current = this.currentUser();
        const nextUser: AuthUser = {
          ...user,
          permissions: user.permissions ?? current?.permissions ?? [],
        };

        localStorage.setItem(AUTH_USER_KEY, JSON.stringify(nextUser));
        this.currentUser.set(nextUser);
      }),
    );
  }

  refreshToken(): Observable<RefreshTokenResponse> {
    const refreshToken = localStorage.getItem(AUTH_REFRESH_TOKEN_KEY);

    return this.http.post<ApiResponse<RefreshTokenResponse>>(this.refreshUrl, { refreshToken }).pipe(
      map((response) => response.data),
      tap((response) => {
        this.persistSession(response);
      }),
    );
  }

  renewSession(): Observable<AuthUser> {
    return this.refreshToken().pipe(map((response) => this.toAuthUser(response)));
  }

  signUp(payload: SignUpRequest): Observable<AuthUser> {
    return this.http
      .post<ApiResponse<AuthUser>>(this.userUrl, payload)
      .pipe(map((response) => response.data));
  }

  updateAccount(payload: UpdateAccountRequest): Observable<AuthUser> {
    const userId = this.currentUser()?.userId;

    if (!userId) {
      throw new Error('No hay usuario autenticado.');
    }

    return this.http.put<ApiResponse<AuthUser>>(`${this.userUrl}/${userId}`, payload).pipe(
      map((response) => response.data),
      tap((user) => {
        const current = this.currentUser();
        const nextUser: AuthUser = {
          ...user,
          permissions: current?.permissions ?? user.permissions ?? [],
        };

        localStorage.setItem(AUTH_USER_KEY, JSON.stringify(nextUser));
        this.currentUser.set(nextUser);
      }),
    );
  }

  deleteAccount(): Observable<boolean> {
    const userId = this.currentUser()?.userId;

    if (!userId) {
      throw new Error('No hay usuario autenticado.');
    }

    return this.http.delete<ApiResponse<boolean>>(`${this.userUrl}/${userId}`).pipe(
      map((response) => response.data),
      tap(() => {
        localStorage.removeItem(AUTH_TOKEN_KEY);
        localStorage.removeItem(AUTH_REFRESH_TOKEN_KEY);
        localStorage.removeItem(AUTH_USER_KEY);
        this.currentUser.set(null);
        this.router.navigate(['/login']);
      }),
    );
  }

  registerInit(email: string): Observable<string> {
    return this.http
      .post<ApiResponse<string>>(`${environment.apiUrl}/auth/register/init`, { email })
      .pipe(map((response) => response.data));
  }

  registerValidate(token: string): Observable<{ email: string }> {
    return this.http
      .get<ApiResponse<{ email: string }>>(`${environment.apiUrl}/auth/register/validate/${token}`)
      .pipe(map((response) => response.data));
  }

  registerComplete(
    token: string,
    payload: { email: string; username: string; password: string },
  ): Observable<unknown> {
    return this.http
      .post<ApiResponse<unknown>>(`${environment.apiUrl}/auth/register/complete/${token}`, payload)
      .pipe(map((response) => response.data));
  }

  recoverPasswordSendOtp(email: string): Observable<string> {
    return this.http
      .post<ApiResponse<string>>(`${environment.apiUrl}/auth/recoverPassword`, { email })
      .pipe(map((response) => response.data));
  }

  recoverPassword(code: string, newPassword: string): Observable<string> {
    return this.http
      .post<ApiResponse<string>>(`${environment.apiUrl}/auth/recoverPassword/${code}`, { newPassword })
      .pipe(map((response) => response.data));
  }

  changePassword(currentPassword: string, newPassword: string): Observable<string> {
    return this.http
      .post<ApiResponse<string>>(`${environment.apiUrl}/auth/changePassword`, {
        currentPassword,
        newPassword,
      })
      .pipe(map((response) => response.data));
  }

  logout(): void {
    localStorage.removeItem(AUTH_TOKEN_KEY);
    localStorage.removeItem(AUTH_REFRESH_TOKEN_KEY);
    localStorage.removeItem(AUTH_USER_KEY);
    this.currentUser.set(null);
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return localStorage.getItem(AUTH_TOKEN_KEY);
  }

  isAuthenticated(): boolean {
    return !!this.getToken() && !!this.currentUser()?.isActive;
  }

  getSessionSummary(): { token: string | null; user: AuthUser | null } {
    return {
      token: this.getToken(),
      user: this.currentUser(),
    };
  }

  private loadUserFromStorage(): AuthUser | null {
    const userJson = localStorage.getItem(AUTH_USER_KEY);
    if (!userJson) {
      return null;
    }

    try {
      return JSON.parse(userJson) as AuthUser;
    } catch {
      return null;
    }
  }

  private persistSession(response: LoginResponse): void {
    if (!response.token) {
      throw new Error('La API no devolvio un token de autenticacion.');
    }

    const user = this.toAuthUser(response);

    localStorage.setItem(AUTH_TOKEN_KEY, response.token);
    localStorage.setItem(AUTH_REFRESH_TOKEN_KEY, response.refreshToken);
    localStorage.setItem(AUTH_USER_KEY, JSON.stringify(user));
    this.currentUser.set(user);
  }

  private toAuthUser(response: LoginResponse): AuthUser {
    return {
      userId: response.userId,
      username: response.username,
      email: response.email,
      statusId: response.statusId,
      statusName: response.statusName,
      isActive: response.isActive,
      roles: response.roles ?? [],
      permissions: response.permissions ?? [],
    };
  }
}
