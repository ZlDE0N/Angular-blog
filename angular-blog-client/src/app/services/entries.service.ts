import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Entry } from '../models/entry.model';
import { map, switchMap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class EntriesService {
    private apiUrl = 'https://localhost:5001/api/entity'; // Nueva URL de la API
    private postData = [];
    constructor(private http: HttpClient) {}

    // Método existente para obtener todas las entradas
    getEntries(): Observable<Entry[]> {
        return this.http.get<Entry[]>(this.apiUrl).pipe(
          map(response => {
            if (response && Array.isArray(response)) {
              return response.filter(entry => entry.estado); // Filtra solo las entradas con estado true
            } else {
              console.error('La respuesta de la API no contiene un array de entradas válido:', response);
              return []; // Retorna un array vacío si no hay datos válidos
            }
          })
        );
      }

    // Nuevo método para obtener una entrada por ID
    getEntryById(id: number): Observable<Entry> {
      return this.http.get<Entry>(`${this.apiUrl}/${id}`).pipe(
        map(response => {
          if (response) {
            return response; // Devuelve la entrada
          } else {
            console.error('La respuesta de la API no contiene una entrada válida:', response);
            throw new Error('Entrada no válida'); // Lanza un error si no hay datos válidos
          }
        })
      );
    }

    // Método para crear una nueva entrada
    createEntry(entry: Entry): Observable<Entry> {
      return this.http.post<Entry>(this.apiUrl, entry);
    }

    getEntriesByCategoryId(categoryId: number): Observable<Entry[]> {
      return this.http.get<Entry[]>(`${this.apiUrl}?idCategories=${categoryId}`).pipe(
        map(response => {
          if (response && Array.isArray(response)) {
            return response; // Devuelve el array de entradas
          } else {
            console.error('La respuesta de la API no contiene un array de entradas válido:', response);
            return []; // Retorna un array vacío si no hay datos válidos
          }
        })
      );
    }

    updateEntry(entry: Entry): Observable<Entry> {
        return this.http.put<Entry>(`${this.apiUrl}/${entry.idEntriesBlog}`, entry).pipe(
          map(response => {
            return response; // Retorna la entrada actualizada
          })
        );
  }

  deleteEntry(id: number): Observable<Entry> {
    return this.http.get<Entry>(`${this.apiUrl}/${id}`).pipe(
      switchMap(entry => {
        entry.estado = false; // Cambia el estado a false
        return this.updateEntry(entry); // Actualiza la entrada con el nuevo estado
      })
    );
  }

  setPostData(data: any): void {
    this.postData = data;
  }
  getPostData(): any {
    return this.postData;
  }


}
