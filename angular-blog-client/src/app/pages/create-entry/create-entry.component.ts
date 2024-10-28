import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { EntriesService } from '../../services/entries.service';
import { CategoryService } from '../../services/category.service';
import { Category } from '../../models/category.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-create-entry',
  standalone: true,
  imports:[CommonModule, ReactiveFormsModule],
  templateUrl: './create-entry.component.html',
  styleUrls: ['./create-entry.component.css'],
})
export class CreateEntryComponent implements OnInit {
  entryForm: FormGroup;
  categories: Category[] = [];

  constructor(
    private fb: FormBuilder,
    private entriesService: EntriesService,
    private categoryService: CategoryService,
    private router: Router
  ) {
    this.entryForm = this.fb.group({
      title: ['', Validators.required],
      content: ['', Validators.required],
      author: ['', Validators.required],
      publicationDate: ['', Validators.required],
      idCategories: ['', Validators.required], // Campo para la categoría
    });
  }

  ngOnInit(): void {
    // Obtener las categorías al cargar el componente
    this.categoryService.getCategories().subscribe((categories) => {
      this.categories = categories;
    });
  }

  onSubmit(): void {
    if (this.entryForm.valid) {
      console.log(this.entryForm.value)
      const newEntry = {
        ...this.entryForm.value,
        idCategories: [parseInt(this.entryForm.value.idCategories, 10)],
        estado: true, // Siempre que el estado sea verdadero al crear una nueva entrada
      };
      this.entriesService.createEntry(newEntry).subscribe(
        (response) => {
          console.log('Entrada creada:', response);
          this.router.navigate(['/']); // Redirigir a la página principal o a donde desees
        },
        (error) => {
          console.error('Error al crear la entrada:', error);
        }
      );
    } else {
      console.error('Formulario no válido');
    }
  }
}
