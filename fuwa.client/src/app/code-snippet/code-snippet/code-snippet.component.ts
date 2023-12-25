import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CodeSnippet } from 'src/app/models/codeSnippet';
import { CodeSnippetsService } from 'src/app/services/code-snippets.service';

@Component({
  selector: 'app-code-snippet',
  templateUrl: './code-snippet.component.html',
  styleUrls: ['./code-snippet.component.css']
})
export class CodeSnippetComponent {
  codeSnippet: CodeSnippet | undefined;
  constructor(private codeSnippetsService: CodeSnippetsService,
              private route: ActivatedRoute
              ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const userTag = params['user'];
      const codeSnippet = params['snippet'];
      this.getCodeSnippet(userTag, codeSnippet);
    })
  }

  private getCodeSnippet(userTag: string, codeSnippetName: string) {
    this.codeSnippetsService.getCodeSnippet(userTag, codeSnippetName).subscribe({
      next: (response) => {
        this.codeSnippet = response;
      },
      error: (error) => {
        console.error("Error while fetching code snippet: ", error);
      }
    });
  }
}
