import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CodeSnippetShort } from 'src/app/models/codeSnippetShort';
import { UsersService } from 'src/app/services/users.service';

@Component({
  selector: 'app-user-likes',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './user-likes.component.html',
  styleUrl: './user-likes.component.css'
})
export class UserLikesComponent {
  @Input() userTag: string = "";
  likes: CodeSnippetShort[] | undefined;

  constructor(private usersService: UsersService) { }

  ngOnInit(): void {
    this.usersService.getUserLikes(this.userTag).subscribe({
      next: (response) => {
        this.likes = response;
      },
      error: (error) => {
        console.error("Error while fetching liked code snippets: ", error);
      }
    });
  }
}
