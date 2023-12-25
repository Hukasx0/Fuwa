import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Post } from '../models/post';
import { PostComment } from '../models/postComment';
import { NewPostComment } from '../models/newPostComment';
import { NewPost } from '../models/newPost';

@Injectable({
  providedIn: 'root'
})
export class PostsService {
  postsUrl = "api/Post";
  commentsUrl = "comments";
  apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  public getPosts(): Observable<Post[]> {
    return this.http.get<Post[]>(`${this.apiUrl}/${this.postsUrl}`);
  }

  public getPost(id: number): Observable<Post> {
    return this.http.get<Post>(`${this.apiUrl}/${this.postsUrl}/${id}`);
  }

  public postPost(newPost: NewPost): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/${this.postsUrl}`, newPost);
  }

  public editPost(id: number, changedPost: NewPost): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${this.postsUrl}/${id}`, changedPost);
  }

  public deletePost(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${this.postsUrl}/${id}`);
  }



  public getPostComments(id: number): Observable<PostComment[]> {
    return this.http.get<PostComment[]>(`${this.apiUrl}/${this.postsUrl}/${id}/${this.commentsUrl}`);
  }

  public postPostComment(postid: number, text: NewPostComment): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/${this.postsUrl}/${postid}/${this.commentsUrl}`, text);
  }

  public editPostComment(id: number, changedComment: NewPostComment, commentid: number): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${this.postsUrl}/${id}/${this.commentsUrl}/${commentid}`, changedComment);
  }
  
  public deletePostComment(id: number, commentid: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${this.postsUrl}/${id}/${this.commentsUrl}/${commentid}`);
  }
}
