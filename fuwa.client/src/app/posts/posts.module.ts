import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PostsComponent } from './posts/posts.component';
import { RouterLink } from '@angular/router';
import { TimeAgoPipe } from '../time-ago.pipe';

import { CardModule } from 'primeng/card';
import { PanelModule } from 'primeng/panel';
import { AvatarModule } from 'primeng/avatar';



@NgModule({
  declarations: [
    PostsComponent
  ],
  imports: [
    CommonModule,
    RouterLink,
    TimeAgoPipe,
    CardModule,
    AvatarModule,
    PanelModule
  ]
})
export class PostsModule { }
