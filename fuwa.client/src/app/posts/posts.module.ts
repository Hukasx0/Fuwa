import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PostsComponent } from './posts/posts.component';
import { RouterLink } from '@angular/router';
import { TimeAgoPipe } from '../time-ago.pipe';



@NgModule({
  declarations: [
    PostsComponent
  ],
  imports: [
    CommonModule,
    RouterLink,
    TimeAgoPipe
  ]
})
export class PostsModule { }
