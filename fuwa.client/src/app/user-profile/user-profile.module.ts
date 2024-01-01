import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { UserCommentsComponent } from './user-comments/user-comments.component';
import { UserLikesComponent } from './user-likes/user-likes.component';
import { UserPostsComponent } from './user-posts/user-posts.component';
import { UserCodeSnippetsComponent } from './user-code-snippets/user-code-snippets.component';
import { TabViewModule } from 'primeng/tabview';
import { TabMenuModule } from 'primeng/tabmenu';
import { UserMainComponent } from './user-main/user-main.component';
import { ImageModule } from 'primeng/image';



@NgModule({
  declarations: [
    UserProfileComponent
  ],
  imports: [
    CommonModule,
    UserCommentsComponent,
    UserLikesComponent,
    UserPostsComponent,
    UserMainComponent,
    UserCodeSnippetsComponent,
    UserLikesComponent,
    TabViewModule,
    TabMenuModule,
    ImageModule
  ]
})
export class UserProfileModule { }
