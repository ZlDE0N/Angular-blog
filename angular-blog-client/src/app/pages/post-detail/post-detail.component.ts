import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EntriesService } from '../../services/entries.service';
import { Entry } from '../../models/entry.model';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CategoryService } from '../../services/category.service';
import { Category } from '../../models/category.model';

@Component({
  selector: 'app-post-detail',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './post-detail.component.html',
  styleUrls: ['./post-detail.component.css'],
})
export class PostDetailComponent implements OnInit {
  postForm: FormGroup;
  loading = true;
  error: string | null = null;
  categories: any[] = []; // Asume que tienes un servicio para obtener categorías
  entryId!: number;
  entries: any = [];

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private entriesService: EntriesService,
    private categoryService: CategoryService
  ) {
    this.postForm = this.fb.group({
      title: ['', Validators.required],
      author: ['', Validators.required],
      publicationDate: ['', Validators.required],
      content: ['', Validators.required],
      idCategories: [null, Validators.required]
    });
  }

  ngOnInit(): void {
    this.entryId = +this.route.snapshot.paramMap.get('id')!; // Asume que estás pasando el ID en la ruta
    this.entries = this.entriesService.getPostData();
    this.loadEntry();
    this.loadCategories(); // Método para cargar categorías si es necesario
  }

  loadEntry(): void {
    this.entriesService.getEntryById(this.entryId).subscribe(
      (entry: Entry) => {
        // Formatear la fecha
      entry.publicationDate = new Date(entry.publicationDate)
        .toISOString()
        .split('T')[0];
        this.postForm.patchValue(entry); // Cargar los datos en el formulario
        this.loading = false;
      },
      (error) => {
        this.error = 'Error al cargar la entrada';
        this.loading = false;
      }
    );
  }

  loadCategories(): void {
    // Aquí podrías cargar las categorías usando un servicio
    this.categoryService.getCategories().subscribe((categories) => {
      this.categories = categories;
    });
  }

  updateEntry(): void {
    if (this.postForm.valid) {
      this.entriesService.updateEntry({ ...this.postForm.value , idCategories: [this.postForm.value.idCategories], id: this.entryId, idEntriesBlog: this.entries.idEntriesBlog }).subscribe(
        () => {
          this.router.navigate(['/']); // Redirecciona a la ruta inicial
        },
        (error) => {
          this.error = 'Error al actualizar la entrada';
        }
      );
    }
  }

  deleteEntry(): void {
    this.entriesService.deleteEntry(this.entryId).subscribe(
      () => {
        this.router.navigate(['/']); // Redirecciona a la ruta inicial
      },
      (error) => {
        this.error = 'Error al eliminar la entrada';
      }
    );
  }
}
