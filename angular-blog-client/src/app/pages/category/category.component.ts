import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { EntriesService } from '../../services/entries.service';
import { Entry } from '../../models/entry.model';
import { PostCardComponent } from '../../shared/components/post-card/post-card.component';
import { SharedModule } from '../../shared/shared.module';

@Component({
  selector: 'app-category',
  standalone: true,
  imports: [SharedModule],
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.css']
})
export class CategoryComponent implements OnInit {
  categoryName: string = ''; // Inicializar para evitar errores
  categoryId: number = 0; // Inicializar en 0
  entries: Entry[] = []; // Para almacenar las entradas relacionadas

  constructor(private route: ActivatedRoute, private entriesService: EntriesService) {}

  ngOnInit(): void {
    // Capturar parámetros de la URL
    this.route.paramMap.subscribe(params => {
      this.categoryName = params.get('categoryName') || '';
      this.categoryId = +params.get('categoryId')!; // Convertir a número

      // Obtener entradas relacionadas a la categoría
      this.entriesService.getEntriesByCategoryId(this.categoryId).subscribe(
        (entries) => {
          // Filtrar solo las entradas que tienen el idCategories igual a categoryId
          this.entries = entries.filter(entry => entry.idCategories === this.categoryId && entry.estado);
        },
        (error) => {
          console.error('Error al cargar entradas:', error);
        }
      );
    });
  }
}