import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { NewPostComponent } from './new-post/new-post.component';

const routes: Routes = [
  {
    path: 'posts/new',
    component: NewPostComponent
  }
]

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class NewPostRoutingModule { }
