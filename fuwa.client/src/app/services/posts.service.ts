import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Post } from '../models/post';
import { PostComment } from '../models/postComment';

@Injectable({
  providedIn: 'root'
})
export class PostsService {
  getPostsUrl = "api/Post";
  getCommentsUrl = "comments";
  apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  public getPosts(): Observable<Post[]> {
    return this.http.get<Post[]>(`${this.apiUrl}/${this.getPostsUrl}`);
  }

  public getPost(id: number): Observable<Post> {
    return this.http.get<Post>(`${this.apiUrl}/${this.getPostsUrl}/${id}`);
  }

  public getPostComments(id: number): Observable<PostComment[]> {
    return this.http.get<PostComment[]>(`${this.apiUrl}/${this.getPostsUrl}/${id}/${this.getCommentsUrl}`);
  }
}
