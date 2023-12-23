import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Login } from '../models/login';
import { Register } from '../models/register';
import { JwtAuth } from '../models/jwtAuth';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  registerUrl = 'api/Auth/register';
  loginUrl = 'api/Auth/login';
  apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  public register(userData: Register): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/${this.registerUrl}`, userData);
  }

  public login(userData: Login): Observable<JwtAuth> {
    return this.http.post<JwtAuth>(`${this.apiUrl}/${this.loginUrl}`, userData);
  }
}
