import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { CategoryNavbarComponent } from './components/category-navbar/category-navbar.component';
import { PostCardComponent } from './components/post-card/post-card.component';
import { FormatDatePipe } from '../pipes/format-date.pipe';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    CategoryNavbarComponent,
    HeaderComponent,
    FooterComponent,
    PostCardComponent,
    FormatDatePipe,
  ],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule, 
  ],
  exports: [
    CategoryNavbarComponent,
    HeaderComponent,
    FooterComponent,
    PostCardComponent,
    CommonModule,
    FormatDatePipe
  ]
})
export class SharedModule { }
