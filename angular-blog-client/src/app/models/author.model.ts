export interface AuthorResponse {
    value: string[]; // Solo queremos la lista de autores
    error: string | null;
    isSuccess: boolean;
    message: string;
  }
   