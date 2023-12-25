import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { CodeSnippet } from '../models/codeSnippet';
import { Observable } from 'rxjs';
import { NewCodeSnippet } from '../models/newCodeSnippet';

@Injectable({
  providedIn: 'root'
})
export class CodeSnippetsService {
  apiUrl = environment.apiUrl;
  userUrl = "api/User";
  codeSnippetsUrl = "codeSnippets"

  constructor(private http: HttpClient) { }

  public getCodeSnippet(userTag: string, snippetName: string): Observable<CodeSnippet> {
    return this.http.get<CodeSnippet>(`${this.apiUrl}/${this.userUrl}/${userTag}/codeSnippets/${snippetName}`);
  }

  public postCodeSnippet(newCodeSnippet: NewCodeSnippet): Observable<any> {
    return this.http.post<any>('/api/CodeSnippet', newCodeSnippet);
  }

  public editCodeSnippet(userTag: string, snippetName: string, changedCodeSnippet: NewCodeSnippet): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${this.userUrl}/${userTag}/codeSnippets/${snippetName}`, changedCodeSnippet);
  }

  public deleteCodeSnippet(userTag: string, snippetName: string): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${this.userUrl}/${userTag}/codeSnippets/${snippetName}`);
  }
}
