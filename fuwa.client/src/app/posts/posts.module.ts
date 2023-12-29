import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PostsComponent } from './posts/posts.component';
import { RouterLink } from '@angular/router';
import { TimeAgoPipe } from '../time-ago.pipe';

import { CardModule } from 'primeng/card';



@NgModule({
  declarations: [
    PostsComponent
  ],
  imports: [
    CommonModule,
    RouterLink,
    TimeAgoPipe,
    CardModule
  ]
})
export class PostsModule { }
