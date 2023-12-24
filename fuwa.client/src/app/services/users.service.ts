import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '../models/user';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  getUserUrl = "api/User"
  apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  public getUser(tag: string): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/${this.getUserUrl}/${tag}`);
  }
}
