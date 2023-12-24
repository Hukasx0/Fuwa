import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PostsComponent } from './posts/posts.component';
import { RouterLink } from '@angular/router';



@NgModule({
  declarations: [
    PostsComponent
  ],
  imports: [
    CommonModule,
    RouterLink
  ]
})
export class PostsModule { }
