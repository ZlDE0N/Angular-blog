import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user.model';


@Component({
  selector: 'app-edit-information',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './edit-information.component.html',
  styleUrl: './edit-information.component.css'
})
export class EditInformationComponent implements OnInit {
  userForm: FormGroup;
  userId!: number; // Declare userId sin inicializar aquí

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService
  ) {
    this.userForm = this.fb.group({
      username: ['', Validators.required],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.userId = Number(this.route.snapshot.paramMap.get('id')); // Asignar el ID al inicializar
    this.userService.getUser(this.userId).subscribe(user => {
      this.userForm.patchValue({
        username: user.username,
        firstName: user.firstName,
        lastName: user.lastName
      });
    });
  }

  onSubmit(): void {
    const updatedUser: User = {
      id: this.userId,  // Asegúrate de que este ID sea correcto
      username: this.userForm.value.username,  // Debes tener el nombre de usuario
      password: '',  // Puedes dejarlo vacío si no es necesario actualizar la contraseña
      firstName: this.userForm.value.firstName,  // Nombre
      lastName: this.userForm.value.lastName,  // Apellido
      email: '',  // Proporciona el email correspondiente
      createdAt: new Date().toISOString(),  // Asegúrate de enviar un formato de fecha válido
      updatedAt: new Date().toISOString(),  // Asegúrate de enviar un formato de fecha válido
      estado: true  // Puedes ajustarlo según tu lógica
    };

    // Llama al servicio para actualizar el usuario
    this.userService.updateUser(updatedUser).subscribe(() => {
      this.router.navigate(['/']); // Navegar a la ruta inicial
    }, error => {
      console.error('Error al actualizar el usuario:', error);
    });
}
}