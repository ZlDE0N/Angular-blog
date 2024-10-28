import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { Category } from '../models/category.model';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private apiUrl = `${environment.apiUrl}/category`; // URL de la API
  private categoriesSubject = new BehaviorSubject<Category[]>([]);
  categories$ = this.categoriesSubject.asObservable();

  constructor(private http: HttpClient) {}

  // Devuelve solo los nombres de las categorías
  getCategories(): Observable<Category[]> {
    return this.http.get<{ value: Category[] }>(this.apiUrl).pipe(
      map(response => response.value) // Devuelve los objetos completos de categoría
    );
  }

  // Método para cargar categorías y actualizar el BehaviorSubject
  loadCategories(): void {
    this.getCategories().subscribe(categories => {
      this.categoriesSubject.next(categories); // Emitir las categorías nuevamente
    });
  }

  // Método para crear una categoría
  createCategory(category: Category): Observable<Category> {
    return this.http.post<Category>(this.apiUrl, category).pipe(
      map(response => {
        this.loadCategories(); // Cargar categorías después de crear una nueva
        return response;
      })
    );
  }

  updateCategory(category: Category): Observable<Category> {
    return this.http.put<Category>(`${this.apiUrl}/${category.id}`, category).pipe(
      map(response => {
        this.loadCategories(); // Recargar categorías después de la actualización
        return response;
      })
    );
  }

  deleteCategory(categoryId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${categoryId}`).pipe(
      map(() => {
        this.loadCategories(); // Recargar categorías después de la eliminación
      })
    );
  }
}
