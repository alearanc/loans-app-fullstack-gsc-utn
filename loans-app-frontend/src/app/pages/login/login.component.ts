import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { LoginService } from 'src/app/services/login.service';
import { LoginRequest } from 'src/app/interfaces/loginRequest.interface';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  loginForm: FormGroup;
  loginError: string = '';

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private _snackBar: MatSnackBar,
    private loginService: LoginService
  ) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  //  AVISO: Al parecer el login funciona, puesto que entra al método next: y
  //  devuelve el token utilizado para entrar. Al probar el login mediante Postman
  //  no hubo inconveniente alguno.

  //  Al parecer el problema es CORS. Busqué por varios sitios con posibles
  //  soluciones como modificar el backend agregando Cors y hasta agregando aquí un
  //  archivo proxy.conf.json para ver si podía acceder pero no hubo caso.

  //  Queda pendiente para solucionarlo.
  login() {
    if (this.loginForm.valid) {
      this.loginService.login(this.loginForm.value as LoginRequest).subscribe({
        next: (userData) => {
          console.log(userData);
          console.info("Login funcionando")
        },
        error: (errorData) => {
          this.loginError = errorData;
        },
        complete: () => {
          this.router.navigateByUrl('/dashboard');
          this.loginForm.reset();
        },
      });
    } else {
      this.showAlert('Datos incorrectos', 'Error');
    }
  }

  showAlert(message: string, action: string) {
    this._snackBar.open(message, action),
      {
        horizontalPosition: 'end',
        verticalPosition: 'top',
        duration: 3000,
      };
  }
}
