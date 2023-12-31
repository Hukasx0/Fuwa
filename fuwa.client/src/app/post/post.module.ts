import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PostComponent } from './post/post.component';
import { RouterLink } from '@angular/router';
import { TimeAgoPipe } from '../time-ago.pipe';

import { FieldsetModule } from 'primeng/fieldset';
import { AvatarModule } from 'primeng/avatar';
import { CardModule } from 'primeng/card';


@NgModule({
  declarations: [
    PostComponent
  ],
  imports: [
    CommonModule,
    RouterLink,
    TimeAgoPipe,
    FieldsetModule,
    AvatarModule,
    CardModule
  ]
})
export class PostModule { }
