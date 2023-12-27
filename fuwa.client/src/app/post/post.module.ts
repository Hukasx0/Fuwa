import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PostComponent } from './post/post.component';
import { RouterLink } from '@angular/router';
import { TimeAgoPipe } from '../time-ago.pipe';


@NgModule({
  declarations: [
    PostComponent
  ],
  imports: [
    CommonModule,
    RouterLink,
    TimeAgoPipe
  ]
})
export class PostModule { }
