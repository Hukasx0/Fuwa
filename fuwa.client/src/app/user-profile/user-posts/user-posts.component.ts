import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Post } from 'src/app/models/post';
import { UsersService } from 'src/app/services/users.service';
import { TimeAgoPipe } from 'src/app/time-ago.pipe';

@Component({
  selector: 'app-user-posts',
  standalone: true,
  imports: [RouterLink, TimeAgoPipe],
  templateUrl: './user-posts.component.html',
  styleUrl: './user-posts.component.css'
})
export class UserPostsComponent {
  @Input() userTag: string = '';
  posts: Post[] | undefined;

  constructor(private usersService: UsersService) { }

  ngOnInit(): void {
    this.usersService.getUserPosts(this.userTag).subscribe({
      next: (response) => {
        this.posts = response;
      },
      error: (error) => {
        console.error("Error while fetching comments: ", error);
      }
    });
  }
}
