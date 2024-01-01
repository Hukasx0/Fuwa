import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Login } from '../models/login';
import { Register } from '../models/register';
import { JwtAuth } from '../models/jwtAuth';
import { Observable, throwError } from 'rxjs';
import { environment } from 'src/environments/environment';
import { catchError, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  registerUrl = 'api/Auth/register';
  loginUrl = 'api/Auth/login';
  apiUrl = environment.apiUrl;

  private isUserLoggedIn = false;

  constructor(private http: HttpClient) { }

  public register(userData: Register): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/${this.registerUrl}`, userData);
  }

  public login(userData: Login): Observable<JwtAuth> {
    return this.http.post<JwtAuth>(`${this.apiUrl}/${this.loginUrl}`, userData)
      .pipe(
        tap(response => {
          localStorage.setItem('auth', response.token);
          this.isUserLoggedIn = true;
        }),
        catchError(error => {
          console.error('Login error:', error);
          return throwError(() => error);
        })
      );
  }

  public isLoggedIn(): boolean {
    return this.isUserLoggedIn;
  }

  public logout(): void {
    localStorage.removeItem('auth');
    this.isUserLoggedIn = false;
  }
}
