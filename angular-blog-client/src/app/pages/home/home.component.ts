import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { EntriesService } from '../../services/entries.service';
import { Entry } from '../../models/entry.model';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterModule, SharedModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  featuredPostsArray: Entry[] = [];
  entryForm: FormGroup;

  constructor(
    private entriesService: EntriesService,
    private fb: FormBuilder
  ) {
    this.entryForm = this.fb.group({
      title: [''],
      content: [''],
      publicationDate: [''],
      author: [''],
      category: [''] // Si decides usar la categoría, puedes rellenar esto en el futuro
    });
  }

  ngOnInit(): void {
    this.entriesService.getEntries().subscribe(
      (entries) => {
        if (Array.isArray(entries)) {
          this.featuredPostsArray = entries.filter(entry => entry.estado);
          // Rellenar el formulario con los datos del primer post como ejemplo
          if (this.featuredPostsArray.length) {
            const firstPost = this.featuredPostsArray[0];
            this.entryForm.patchValue({
              title: firstPost.title,
              content: firstPost.content,
              publicationDate: firstPost.publicationDate,
              author: firstPost.author,
              // category: firstPost.idCategories.join(', ') // Si decides usar las categorías, puedes descomentar esto
            });
          }
        } else {
          console.error('La respuesta de la API no es un array:', entries);
        }
      },
      (error) => {
        console.error('Error al cargar entradas:', error);
      }
    );
  }
}
