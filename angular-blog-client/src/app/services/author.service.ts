import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

export interface AuthorResponse {
  value: string[];
  error: string | null;
  isSuccess: boolean;
  message: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthorService {
  private apiUrl = 'https://localhost:5001/api/blog/GetAuthorCatalog';

  constructor(private http: HttpClient) {}

  getAuthors(): Observable<string[]> {
    return this.http.get<AuthorResponse>(this.apiUrl).pipe(
      map(response => {
        if (response.isSuccess && Array.isArray(response.value)) {
          return response.value; // Devuelve solo la lista de autores
        } else {
          console.error('Error al obtener autores:', response.error);
          return []; // Retorna un array vacío si no hay datos válidos
        }
      })
    );
  }
}
