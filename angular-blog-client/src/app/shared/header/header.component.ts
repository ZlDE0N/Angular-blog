import { Component } from '@angular/core';
import { Router } from '@angular/router'; // Asegúrate de importar Router

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {
  isLoggedIn: boolean = true; // Cambiar según el estado de inicio de sesión
  username: string = 'Usuario'; // Cambia esto por el nombre real del usuario
  userId: number = 1; // Cambia esto por el ID real del usuario

  // Cambiar private a public
  constructor(public router: Router) {} // Inyectar el Router

  changePassword() {
  }

  logout() {
    this.isLoggedIn = false; // Actualizar el estado
  }

  navigateToEditInformation() {
    this.router.navigate(['/edit-information', this.userId]);
  }

  isLoginRoute(): boolean {
    return this.router.url === '/login' || this.router.url === '/register';
  }

  goToRegister() {
    this.router.navigate(['/register']).then(success => {
      if (!success) {
        console.error('Navigation to register failed');
      }
    });

  }

  goToLogin() {
    this.router.navigate(['/login']);
  }
  
}
