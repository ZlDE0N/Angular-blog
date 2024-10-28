export interface Entry {
    id: number;
    idEntriesBlog:number;
    idCategories: number[] | number, // Cambiado a solo un número, ya que ahora es un único ID
    categoriaName: string; // Nombre de la categoría
    title: string;
    content: string;
    author: string;
    publicationDate: string; // Puede ser Date dependiendo de cómo lo manejes
    estado: boolean;
  }
  