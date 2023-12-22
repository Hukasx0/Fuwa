import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { NewCodeSnippetComponent } from './new-code-snippet/new-code-snippet.component';

const routes: Routes = [
  {
    path: 'new',
    component: NewCodeSnippetComponent
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
export class NewCodeSnippetRoutingModule { }
