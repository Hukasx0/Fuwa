import { Component, Input } from '@angular/core';
import { CodeSnippet } from 'src/app/models/codeSnippet';
import { CodeSnippetsService } from 'src/app/services/code-snippets.service';

@Component({
  selector: 'app-user-main',
  standalone: true,
  imports: [],
  templateUrl: './user-main.component.html',
  styleUrl: './user-main.component.css'
})
export class UserMainComponent {
  @Input() userTag: string = '';
  readmeMD: CodeSnippet | undefined;

  constructor(private codeSnippetsService: CodeSnippetsService) { }

  ngOnInit(): void {
    this.codeSnippetsService.getCodeSnippet(this.userTag, "README.md").subscribe({
      next: (response) => {
        this.readmeMD = response;
      },
      error: (error) => {
        console.error("Error while fetching README.md: ", error);
      }
    })
  }
}
