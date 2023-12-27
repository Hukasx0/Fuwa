import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { UserCommentsComponent } from './user-comments/user-comments.component';
import { UserLikesComponent } from './user-likes/user-likes.component';
import { UserPostsComponent } from './user-posts/user-posts.component';
import { UserCodeSnippetsComponent } from './user-code-snippets/user-code-snippets.component';



@NgModule({
  declarations: [
    UserProfileComponent
  ],
  imports: [
    CommonModule,
    UserCommentsComponent,
    UserLikesComponent,
    UserPostsComponent,
    UserCodeSnippetsComponent,
    UserLikesComponent
  ]
})
export class UserProfileModule { }
