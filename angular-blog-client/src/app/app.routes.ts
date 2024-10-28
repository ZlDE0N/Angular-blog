// app-routing.module.ts
import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { CategoryComponent } from './pages/category/category.component';
import { CreateCategoryComponent } from './pages/create-category/create-category.component';
import { EditCategoryComponent } from './pages/edit-category/edit-category.component';
import { PostDetailComponent } from './pages/post-detail/post-detail.component'; // Importa tu componente
import { CreateEntryComponent } from './pages/create-entry/create-entry.component';
import { AuthorPostsComponent } from './pages/author-posts/author-posts.component';
import { EditInformationComponent } from './pages/edit-information/edit-information.component';
import { AuthGuard } from './services/auth.guard';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';

export const routes: Routes = [
  { path: '', component: HomeComponent, canActivate: [AuthGuard] },
  { path: 'category/:categoryName/:categoryId', component: CategoryComponent, canActivate: [AuthGuard] },
  { path: 'create-category', component: CreateCategoryComponent, canActivate: [AuthGuard] },
  { path: 'edit-category/:id', component: EditCategoryComponent, canActivate: [AuthGuard] },
  { path: 'post/:id', component: PostDetailComponent, canActivate: [AuthGuard] }, 
  { path: 'create-entry', component: CreateEntryComponent, canActivate: [AuthGuard] },
  { path: 'author/:author', component: AuthorPostsComponent, canActivate: [AuthGuard] },
  { path: 'edit-information/:id', component: EditInformationComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
];
