import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../shared/shared.module';
import { HomeComponent } from './home/home.component';
import { PostCardComponent } from '../shared/components/post-card/post-card.component';

@NgModule({
  declarations: [
    // HomeComponent,
    // PostCardComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    // SharedModule
  ],
  exports:[

  ]
})
export class PagesModule { }
