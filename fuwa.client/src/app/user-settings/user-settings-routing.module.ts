import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { UserSettingsComponent } from './user-settings/user-settings.component';

const routes: Routes = [
  {
    path: 'settings',
    component: UserSettingsComponent
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
export class UserSettingsRoutingModule { }
