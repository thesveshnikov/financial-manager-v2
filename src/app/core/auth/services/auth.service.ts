import { inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { tap } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';

import { environment } from '../../../../environments/environment';

import { RegisterUser } from '../models/register-user.model';
import { AuthResponse } from '../models/auth-response.model';
import { Tokens } from '../models/tokens.model';
import { Login } from '../models/login.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly jwtHelper = inject(JwtHelperService);

  private base = environment.apiUrl;

  userName = signal(localStorage.getItem('userName') || '');

  get accessToken() {
    return localStorage.getItem('accessToken') || '';
  }

  get refreshToken() {
    return localStorage.getItem('refreshToken') || '';
  }

  get isLoggedIn(): boolean {
    const token = this.accessToken;

    return !!token && !this.jwtHelper.isTokenExpired(token);
  }

  register(data: RegisterUser) {
    return this.http
      .post<AuthResponse>(`${this.base}/api/users`, data)
      .pipe(tap((r) => this.save(r)));
  }

  login(loginData: Login) {
    return this.http
      .post<AuthResponse>(`${this.base}/api/authentication`, loginData)
      .pipe(tap((r) => this.save(r)));
  }

  logout() {
    ['accessToken', 'refreshToken', 'userName'].forEach((k) => localStorage.removeItem(k));
    this.userName.set('');
  }

  refresh() {
    return this.http
      .post<Tokens>(`${this.base}/api/authentication/token/refresh`, {
        refreshToken: this.refreshToken,
      })
      .pipe(tap((t) => this.saveTokens(t)));
  }

  save(r: AuthResponse) {
    localStorage.setItem('userName', r.name);
    this.userName.set(r.name);
    this.saveTokens(r.tokens);
  }

  saveTokens(t: Tokens) {
    localStorage.setItem('accessToken', t.accessToken);
    localStorage.setItem('refreshToken', t.refreshToken);
  }
}
