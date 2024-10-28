import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideRouter, RouterModule } from '@angular/router';
import { routes } from './app/app.routes'; // Asegúrate de tener tus rutas definidas
import { authInterceptor } from './app/services/auth.interceptor';
import { provideZoneChangeDetection } from '@angular/core';


bootstrapApplication(AppComponent, {
  providers: [
    provideRouter(routes),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideHttpClient(withInterceptors([authInterceptor])), // Usa `withInterceptors` para añadir tu interceptor
  ],
  
})
.catch((err) => console.error(err));
