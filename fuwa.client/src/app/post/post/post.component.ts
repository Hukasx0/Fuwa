import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Post } from 'src/app/models/post';
import { PostsService } from 'src/app/services/posts.service';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.css']
})
export class PostComponent {
  post: Post | undefined;

  constructor(private postsService: PostsService,
              private route: ActivatedRoute
              ) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const postId = params['id'];
      this.getPost(postId);
    });
  }

  private getPost(postId: string): void {
    this.postsService.getPost(Number(postId)).subscribe({
      next: (response) => {
        this.post = response;
      },
      error: (error) => {
        console.error("Error while fetching post: ", error);
      }
    });
  }
}
