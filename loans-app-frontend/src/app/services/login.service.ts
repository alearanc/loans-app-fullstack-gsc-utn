import { Injectable } from '@angular/core';
import { LoginRequest } from '../interfaces/loginRequest.interface';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import {
  Observable,
  catchError,
  throwError,
  BehaviorSubject,
  tap,
  map,
} from 'rxjs';
import { User } from '../interfaces/user.interface';
import { environment } from 'src/environments/envitonments';

@Injectable({
  providedIn: 'root',
})
export class LoginService {
  //  Inicialmente el usuario no está logueado
  currentUserLoginOn: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(
    false
  );
  currentUserData: BehaviorSubject<string> = new BehaviorSubject<string>('');

  constructor(private http: HttpClient) {
    //  busca el token, si token != null --> currentUserLoginOn = true
    this.currentUserLoginOn = new BehaviorSubject<boolean>(
      sessionStorage.getItem('token') != null
    );
    this.currentUserData = new BehaviorSubject<string>(
      sessionStorage.getItem('token') || ''
    );
  }

  login(credentials: LoginRequest): Observable<any> {
    return this.http
      .post<any>(environment.endPoint + 'authentication/login', credentials)
      .pipe(
        tap((userData) => {
          sessionStorage.setItem('token', userData.token);
          //  Se emite la información a todos los componentes suscritos
          this.currentUserData.next(userData.token);
          this.currentUserLoginOn.next(true);
        }),
        //  transforma lo que devuelve la api (devuelve un json)
        map((userData) => userData.token),
        catchError(this.errorHandler)
      );
  }

  logout(): void {
    sessionStorage.removeItem('token');
    this.currentUserLoginOn.next(false);
  }

  private errorHandler(error: HttpErrorResponse) {
    if (error.status === 0) {
      console.error('Se ha producido un error ', error);
    } else {
      console.error('Backend retornó el código de estado ', error);
    }

    return throwError(
      () => new Error('Algo falló. Por favor intente nuevamente')
    );
  }

  //  Propiedades para que los componentes se puedan suscribir
  get userData(): Observable<string> {
    return this.currentUserData.asObservable();
  }

  get userLoginOn(): Observable<boolean> {
    return this.currentUserLoginOn.asObservable();
  }
}
