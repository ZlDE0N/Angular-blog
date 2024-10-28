import { HttpEvent, HttpHandlerFn, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { inject } from '@angular/core';
import { AuthService } from './auth.service'; // Asegúrate de ajustar la ruta de importación
import { Router } from '@angular/router'; // Importa Router
import { catchError } from 'rxjs/operators'; // Importa catchError

export function authInterceptor(req: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<any>> {
  const authService = inject(AuthService);
  const router = inject(Router); // Inyecta el Router

  // Ajusta la verificación de URLs protegidas
  const urlProtected = ["api/auth/login", "api/auth/register"];
  const requestUrl = req.url.split('?')[0]; // Obtiene la URL sin parámetros de consulta
  // Verifica si la URL solicitada está en las protegidas
  if (urlProtected.some(url => requestUrl.includes(url))) {
    return next(req); // Si está en las URLs protegidas, pasa al siguiente manejador
  }

  const authToken = authService.getToken(); // Método para obtener el token

  // Verifica si el token es nulo o inválido
  if (!authToken) {
    console.log('Token no encontrado, redirigiendo a login...');
    router.navigate(['/login']); // Redirige a la ruta de login
    return throwError(() => new Error('Token no encontrado')); // Lanza un error
  }

  // Clona la solicitud para agregar el encabezado de autenticación.
  const newReq = req.clone({
    headers: req.headers.set('Authorization', `Bearer ${authToken}`) // Cambié 'X-Authentication-Token' a 'Authorization'
  });

  return next(newReq).pipe(
    catchError((error) => {
      // Manejo de errores adicionales si es necesario
      if (!error.ok) { // Si el token es inválido (401 Unauthorized)
        router.navigate(['/login']); // Redirige a la ruta de login
      }
      return throwError(() => error); // Propaga el error
    })
  ); // Pasa la nueva solicitud al siguiente manejador
}
