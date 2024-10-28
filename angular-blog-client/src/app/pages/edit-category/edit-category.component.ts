import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CategoryService } from '../../services/category.service';
import { Category } from '../../models/category.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-edit-category',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './edit-category.component.html',
  styleUrls: ['./edit-category.component.css']
})
export class EditCategoryComponent {
  category: Category | null = null; // Inicializar como null

  constructor(
    private route: ActivatedRoute,
    private categoryService: CategoryService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const categoryId = +this.route.snapshot.paramMap.get('id')!; // Obtener el ID de la categoría
    this.categoryService.getCategories().subscribe(categories => {
      this.category = categories.find(cat => cat.id === categoryId) || null; // Buscar la categoría
    });
  }

  updateCategory(): void {
    if (this.category) {
      this.categoryService.updateCategory(this.category).subscribe(() => {
        alert('Categoría actualizada con éxito');
        this.categoryService.loadCategories(); // Recargar categorías
        this.router.navigate(['/']); // Redirigir a donde desees
      }, error => {
        console.error('Error al actualizar la categoría:', error);
      });
    }
  }

  deleteCategory(): void {
    if (this.category) {
      this.categoryService.deleteCategory(this.category.id).subscribe(() => {
        alert('Categoría eliminada con éxito');
        this.categoryService.loadCategories(); // Recargar categorías después de la eliminación
        this.router.navigate(['/']); // Redirigir a la página de inicio
      }, error => {
        console.error('Error al eliminar la categoría:', error);
      });
    }
  }
}
