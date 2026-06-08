import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { switchMap } from 'rxjs';
import { APP_NAME } from '../../../../Core/constants/app.constants';
import { AuthService } from '../../../../Core/auth/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    MatButtonModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
    RouterLink,
  ],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  readonly appName = APP_NAME;
  readonly loading = signal(false);
  readonly signUpLoading = signal(false);
  readonly errorMessage = signal('');
  readonly successMessage = signal('');
  readonly showSignUp = signal(false);
  readonly showRecovery = signal(false);
  readonly recoveryOtpLoading = signal(false);
  readonly recoveryLoading = signal(false);

  readonly loginForm = this.fb.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(4)]],
  });

  readonly signUpForm = this.fb.nonNullable.group({
    username: ['', [Validators.required, Validators.minLength(5)]],
    email: ['', [Validators.required, Validators.email, Validators.minLength(10)]],
    password: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(20)]],
  });

  readonly recoveryForm = this.fb.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    code: ['', Validators.required],
    newPassword: ['', [Validators.required, Validators.minLength(6)]],
  });

  toggleSignUp(): void {
    this.showSignUp.update((value) => !value);
    this.showRecovery.set(false);
    this.errorMessage.set('');
    this.successMessage.set('');
  }

  showRecoverPassword(): void {
    this.showRecovery.set(true);
    this.showSignUp.set(false);
    const loginEmail = this.loginForm.controls.email.value.trim();
    if (loginEmail && !this.recoveryForm.controls.email.value) {
      this.recoveryForm.patchValue({ email: loginEmail });
    }
    this.errorMessage.set('');
    this.successMessage.set('');
  }

  showLogin(): void {
    this.showRecovery.set(false);
    this.showSignUp.set(false);
    this.errorMessage.set('');
    this.successMessage.set('');
  }

  onSubmit(): void {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.loading.set(true);
    this.errorMessage.set('');

    const { email, password } = this.loginForm.getRawValue();

    this.authService.login({ email, password }).subscribe({
      next: () => {
        this.loading.set(false);
        this.goAfterLogin();
      },
      error: (err) => {
        this.loading.set(false);
        this.errorMessage.set(this.resolveError(err, 'Credenciales incorrectas. Verifica tu usuario y contrasena.'));
      },
    });
  }

  onSignUp(): void {
    if (this.signUpForm.invalid) {
      this.signUpForm.markAllAsTouched();
      return;
    }

    const { username, email, password } = this.signUpForm.getRawValue();
    const payload = {
      username: username.trim(),
      email: email.trim(),
      password,
    };

    this.signUpLoading.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');

    this.authService
      .signUp(payload)
      .pipe(switchMap(() => this.authService.login({ email: payload.email, password })))
      .subscribe({
        next: () => {
          this.signUpLoading.set(false);
          this.successMessage.set('Cuenta creada correctamente. Bienvenido a PlayVerse.');
          this.goAfterLogin();
        },
        error: (err) => {
          this.signUpLoading.set(false);
          this.errorMessage.set(this.resolveError(err, 'No se pudo crear la cuenta.'));
        },
      });
  }

  sendRecoveryOtp(): void {
    const emailControl = this.recoveryForm.controls.email;

    if (emailControl.invalid) {
      emailControl.markAsTouched();
      return;
    }

    this.recoveryOtpLoading.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');

    this.authService.recoverPasswordSendOtp(emailControl.value.trim()).subscribe({
      next: () => {
        this.recoveryOtpLoading.set(false);
        this.successMessage.set('Codigo OTP enviado. Revisa tu correo y continua aqui.');
      },
      error: (err) => {
        this.recoveryOtpLoading.set(false);
        this.errorMessage.set(this.resolveError(err, 'No se pudo enviar el codigo OTP.'));
      },
    });
  }

  recoverPassword(): void {
    if (this.recoveryForm.invalid) {
      this.recoveryForm.markAllAsTouched();
      return;
    }

    const raw = this.recoveryForm.getRawValue();

    this.recoveryLoading.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');

    this.authService.recoverPassword(raw.code.trim(), raw.newPassword).subscribe({
      next: () => {
        this.recoveryLoading.set(false);
        this.recoveryForm.reset({ email: '', code: '', newPassword: '' });
        this.showLogin();
        this.successMessage.set('Contrasena actualizada. Ya puedes iniciar sesion.');
      },
      error: (err) => {
        this.recoveryLoading.set(false);
        this.errorMessage.set(this.resolveError(err, 'No se pudo actualizar la contrasena.'));
      },
    });
  }

  private goAfterLogin(): void {
    const returnUrl = this.route.snapshot.queryParamMap.get('returnUrl') ?? '/admin';
    this.router.navigateByUrl(returnUrl);
  }

  private resolveError(error: unknown, fallback: string): string {
    if (typeof error === 'object' && error !== null && 'error' in error) {
      const apiError = (error as { error?: { message?: string; errors?: string[] | Record<string, string[]> } }).error;

      if (Array.isArray(apiError?.errors)) {
        return apiError.errors[0] ?? apiError.message ?? fallback;
      }

      if (apiError?.errors && typeof apiError.errors === 'object') {
        return Object.values(apiError.errors).flat()[0] ?? apiError.message ?? fallback;
      }

      return apiError?.message ?? fallback;
    }

    return fallback;
  }
}
