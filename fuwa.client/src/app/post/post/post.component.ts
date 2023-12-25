import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Post } from 'src/app/models/post';
import { PostComment } from 'src/app/models/postComment';
import { PostsService } from 'src/app/services/posts.service';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.css']
})
export class PostComponent {
  post: Post | undefined;
  postComments: PostComment[] | undefined;

  constructor(private postsService: PostsService,
              private route: ActivatedRoute
              ) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const postId = Number(params['id']);
      this.getPost(postId);
      this.getComments(postId);
    });
  }

  private getPost(postId: number): void {
    this.postsService.getPost(postId).subscribe({
      next: (response) => {
        this.post = response;
      },
      error: (error) => {
        console.error("Error while fetching post: ", error);
      }
    });
  }

  private getComments(postId: number): void {
    this.postsService.getPostComments(postId).subscribe({
      next: (response) => {
        this.postComments = response;
      },
      error: (error) => {
        console.error("Error while fetching comments: ", error);
      }
    });
  }
}
