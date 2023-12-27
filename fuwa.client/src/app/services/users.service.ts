import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '../models/user';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { Post } from '../models/post';
import { PostComment } from '../models/postComment';
import { CodeSnippetShort } from '../models/codeSnippetShort';

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

  public getUserCodeSnippets(tag: string): Observable<CodeSnippetShort[]> {
    return this.http.get<CodeSnippetShort[]>(`${this.apiUrl}/${this.getUserUrl}/${tag}/codeSnippets`);
  }

  public getUserPosts(tag: string): Observable<Post[]> {
    return this.http.get<Post[]>(`${this.apiUrl}/${this.getUserUrl}/${tag}/posts`);
  }

  public getUserPostComments(tag: string): Observable<PostComment[]> {
    return this.http.get<PostComment[]>(`${this.apiUrl}/${this.getUserUrl}/${tag}/comments`);
  }

  public getUserLikes(tag: string): Observable<CodeSnippetShort[]> {
    return this.http.get<CodeSnippetShort[]>(`${this.apiUrl}/${this.getUserUrl}/${tag}/likes`);
  }
}
