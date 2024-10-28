export interface Category {
  id: number;
  name: string;
  description?: string; // Si es opcional, usa el signo de interrogación
  tag?: string; // Igualmente, si es opcional
  createdAt?: string; // Opcional
  updatedAt?: string; // Opcional
  estado?: boolean; // Opcional
}
