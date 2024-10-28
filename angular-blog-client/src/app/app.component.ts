import { Component } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { SharedModule } from './shared/shared.module';
import { authInterceptor } from './services/auth.interceptor';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    SharedModule,
    RouterOutlet, 
  ],
  // providers: [
  //   { provide: HTTP_INTERCEPTORS, useClass: authInterceptor, multi: true },
  // ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'blog-angular-client';

  showNavbar: boolean = true;

  constructor(private router: Router) {}

  ngOnInit(): void {
    // Detectar cambios en la ruta
    this.router.events.subscribe(() => {
      this.checkRoute();
    });
  }

  checkRoute(): void {
    const currentUrl = this.router.url;
    // Verifica si la ruta es /register o /login
    this.showNavbar = !(currentUrl === '/register' || currentUrl === '/login');
  }
}
