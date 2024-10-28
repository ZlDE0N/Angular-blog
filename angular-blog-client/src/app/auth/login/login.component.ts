import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      usernameOrEmail: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      this.authService.login(this.loginForm.value).subscribe({
        next: (response) => {
          // Asegúrate de que el token se obtenga correctamente de la respuesta
          const token = response.tokenDecode.token; // Cambia esto según la estructura de tu respuesta
          if (token) {
            localStorage.setItem('token', token); // Guarda el token en localStorage
          }
          this.router.navigate(['/']); // Redirige al usuario
        },
        error: (err) => console.error('Error al iniciar sesión:', err)
      });
    }
  }
}
