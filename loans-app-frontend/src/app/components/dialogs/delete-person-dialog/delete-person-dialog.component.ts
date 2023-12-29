import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Person } from 'src/app/interfaces/person.interface';

@Component({
  selector: 'app-delete-person-dialog',
  templateUrl: './delete-person-dialog.component.html',
  styleUrls: ['./delete-person-dialog.component.css'],
})
export class DeletePersonDialogComponent {
  constructor(
    private dialogReference: MatDialogRef<DeletePersonDialogComponent>,
    //  info que obtengo al presionar el botón "Editar"
    @Inject(MAT_DIALOG_DATA) public personData: Person
  ) {}

  deleteConfirmation() {
    if (this.personData) {
      this.dialogReference.close('delete')
    }
  }
}
