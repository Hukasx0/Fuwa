import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CodeSnippetShort } from 'src/app/models/codeSnippetShort';
import { UsersService } from 'src/app/services/users.service';
import { TimeAgoPipe } from 'src/app/time-ago.pipe';

@Component({
  selector: 'app-user-code-snippets',
  standalone: true,
  imports: [RouterLink, TimeAgoPipe],
  templateUrl: './user-code-snippets.component.html',
  styleUrl: './user-code-snippets.component.css'
})
export class UserCodeSnippetsComponent {
  @Input() userTag: string = '';
  codeSnippets: CodeSnippetShort[] | undefined;

  constructor(private usersService: UsersService) { }

  ngOnInit(): void {
    this.usersService.getUserCodeSnippets(this.userTag).subscribe({
      next: (response) => {
        this.codeSnippets = response;
        console.log(response);
      },
      error: (error) => {
        console.error("Error while fetching comments: ", error);
      }
    });
  }
}
