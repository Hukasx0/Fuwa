import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeModule } from '../home/home.module';
import { DashboardModule } from '../dashboard/dashboard.module';
import { IndexComponent } from './index/index.component';



@NgModule({
  declarations: [
    IndexComponent
  ],
  imports: [
    CommonModule,
    HomeModule,
    DashboardModule
  ]
})
export class IndexModule { }
