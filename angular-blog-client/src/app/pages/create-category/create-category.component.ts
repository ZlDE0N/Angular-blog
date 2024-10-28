import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CategoryService } from '../../services/category.service';
import { Category } from '../../models/category.model';

@Component({
  selector: 'app-create-category',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './create-category.component.html',
  styleUrls: ['./create-category.component.css']
})
export class CreateCategoryComponent {
  newCategory: Category = { id: 0, name: '', description: '', tag: '', estado: true };

  constructor(private categoryService: CategoryService, private router: Router) {}

  createCategory(): void {
    this.categoryService.createCategory(this.newCategory).subscribe(
      () => {
        alert('Categoría creada con éxito');
        this.categoryService.loadCategories(); // Llama al método para recargar las categorías
        this.router.navigate(['/']); // Redirige a la vista deseada (por ejemplo, al inicio)
      },
      (error) => {
        console.error('Error al crear la categoría:', error);
      }
    );
  }
}
