// user.model.ts
export interface User {
    id: number;         // ID del usuario
    username: string;   // Nombre de usuario
    password: string;   // Contraseña (si es necesario)
    firstName: string;  // Nombre
    lastName: string;   // Apellido
    email: string;      // Correo electrónico
    createdAt: string;  // Fecha de creación en formato ISO
    updatedAt: string;  // Fecha de actualización en formato ISO
    estado: boolean;     // Estado del usuario (activo/inactivo)
  }
  