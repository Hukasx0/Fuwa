import { Component, Input } from '@angular/core';
import { PostComment } from 'src/app/models/postComment';
import { UsersService } from 'src/app/services/users.service';

@Component({
  selector: 'app-user-comments',
  standalone: true,
  imports: [],
  templateUrl: './user-comments.component.html',
  styleUrl: './user-comments.component.css'
})
export class UserCommentsComponent {
  @Input() userTag: string = '';
  comments: PostComment[] | undefined;

  constructor(private usersService: UsersService) { }

  ngOnInit(): void {
    this.usersService.getUserPostComments(this.userTag).subscribe({
      next: (response) => {
        this.comments = response;
      },
      error: (error) => {
        console.error("Error while fetching comments: ", error);
      }
    })
  }
}
