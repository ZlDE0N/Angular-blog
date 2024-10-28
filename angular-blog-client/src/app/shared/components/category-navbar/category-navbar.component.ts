import { Component, OnInit } from '@angular/core';
import { CategoryService } from '../../../services/category.service';
import { AuthorService } from '../../../services/author.service';
import { Category } from '../../../models/category.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-category-navbar',
  templateUrl: './category-navbar.component.html',
  styleUrls: ['./category-navbar.component.css']
})
export class CategoryNavbarComponent implements OnInit {
  categories: Category[] = [];
  authors: string[] = []; // Lista de autores
  isEditing: boolean = false;
  selectedAuthor: string = ''; // Agrega esta línea para definir selectedAuthor

  constructor(private categoryService: CategoryService, private authorService: AuthorService, private router: Router) {}

  ngOnInit(): void {
    this.categoryService.categories$.subscribe(
      (categories: Category[]) => {
        this.categories = categories.filter(category => category.estado);
      },
      (error) => {
        console.error('Error al obtener categorías:', error);
      }
    );

    this.categoryService.loadCategories();
  }

  isEditCategoryRoute(): boolean {
    return this.router.url.includes('/edit-category');
  }

  toggleEditing(): void {
    this.isEditing = !this.isEditing;
  }

  // Método para extraer los autores
  fetchAuthors(): void {
    this.authorService.getAuthors().subscribe(
      (authors) => {
        this.authors = authors; // Almacena los autores en la propiedad
      },
      (error) => {
        console.error('Error al obtener autores:', error);
      }
    );
  }

  selectAuthor(author: string): void {
    if (author) {
      this.router.navigate(['/author', author]); // Navegar a la ruta del autor
    }
  }
}
