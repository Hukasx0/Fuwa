import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { CodeSnippetComponent } from './code-snippet/code-snippet.component';

const routes: Routes = [
  {
    path: ':user/:snippet',
    component: CodeSnippetComponent
  },
]

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class CodeSnippetRoutingModule { }
