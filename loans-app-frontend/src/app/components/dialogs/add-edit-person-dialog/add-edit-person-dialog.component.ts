import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Person } from 'src/app/interfaces/person.interface';
import { PersonService } from 'src/app/services/person.service';

@Component({
  selector: 'app-add-edit-person-dialog',
  templateUrl: './add-edit-person-dialog.component.html',
  styleUrls: ['./add-edit-person-dialog.component.css'],
})
export class AddEditPersonDialogComponent implements OnInit {
  personForm: FormGroup;
  actionTitle: string = 'Nuevo';
  actionButton: string = 'Guardar';

  constructor(
    private fb: FormBuilder,
    private dialogReference: MatDialogRef<AddEditPersonDialogComponent>,
    private _snackBar: MatSnackBar,
    private _personService: PersonService,
    //  info que obtengo al presionar el botón "Editar"
    @Inject(MAT_DIALOG_DATA) public personData: Person
  ) {
    this.personForm = this.fb.group({
      name: ['', Validators.required],
      phoneNumber: ['', Validators.required],
      emailAddress: ['', Validators.required],
    });
  }

  //  Al presionar el botón "Editar" muestra toda la info
  ngOnInit(): void {
    if (this.personData) {
      this.personForm.patchValue({
        name: this.personData.name,
        phoneNumber: this.personData.phoneNumber,
        emailAddress: this.personData.emailAddress,
      });

      this.actionTitle = 'Editar';
      this.actionButton = 'Actualizar';
    }
  }

  showAlert(message: string, action: string) {
    this._snackBar.open(message, action, {
      horizontalPosition: 'end',
      verticalPosition: 'top',
      duration: 3000,
    });
  }

  //  Este método envía una persona a la tabla
  addEditPerson() {
    console.log(this.personForm.value);
    const person: Person = {
      id: this.personData.id,
      name: this.personForm.value.name,
      phoneNumber: this.personForm.value.phoneNumber,
      emailAddress: this.personForm.value.emailAddress,
    };

    if (this.personData == null) {
      this._personService.addPerson(person).subscribe({
        next: (data) => {
          this.showAlert('Persona creada', 'Listo');
          this.dialogReference.close('created');
        },
        error: (e) => {
          this.showAlert('No se pudo crear', 'Error');
        },
      });
    } else {
      this._personService.updatePerson(person).subscribe({
        next: (data) => {
          this.showAlert('Persona actualizada', 'Listo');
          this.dialogReference.close('edited');
        },
        error: (e) => {
          this.showAlert('No se pudo actualizar', 'Error');
        },
      });
    }
  }
}
