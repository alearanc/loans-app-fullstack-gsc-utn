import { AfterViewInit, Component, ViewChild, OnInit, OnDestroy } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Person } from 'src/app/interfaces/person.interface';
import { PersonService } from 'src/app/services/person.service';
import { AddEditPersonDialogComponent } from 'src/app/components/dialogs/add-edit-person-dialog/add-edit-person-dialog.component';
import { MatSnackBar } from '@angular/material/snack-bar';
import { DeletePersonDialogComponent } from 'src/app/components/dialogs/delete-person-dialog/delete-person-dialog.component';
import { LoginService } from 'src/app/services/login.service';

import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent implements AfterViewInit, OnInit {
  displayedColumns: string[] = [
    'id',
    'name',
    'phoneNumber',
    'emailAddress',
    'actions',
  ];
  dataSource = new MatTableDataSource<Person>();

  userLoginOn: boolean = false;
  constructor(
    private _loginService: LoginService,
    private _personService: PersonService,
    public dialog: MatDialog,
    private _snackBar: MatSnackBar
  ) {}

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngOnInit(): void {
    this._loginService.currentUserLoginOn.subscribe({
      next: (userLoginOn) => {
        this.userLoginOn = userLoginOn;
      }
    })

    this.showPersons();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  newPersonDialog() {
    this.dialog
      .open(AddEditPersonDialogComponent, {
        disableClose: true,
        width: '400px',
      })
      .afterClosed()
      .subscribe((result) => {
        if (result === 'created') {
          this.showPersons();
        }
      });
  }

  editPersonDialog(personData: Person) {
    this.dialog
      .open(AddEditPersonDialogComponent, {
        disableClose: true,
        width: '400px',
        data: personData,
      })
      .afterClosed()
      .subscribe((result) => {
        if (result === 'edited') {
          this.showPersons();
        }
      });
  }

  deletePersonDialog(personData: Person) {
    this.dialog
      .open(DeletePersonDialogComponent, {
        disableClose: true,
        data: personData,
      })
      .afterClosed()
      .subscribe((result) => {
        if (result === 'delete') {
          this._personService.delete(personData.id!).subscribe({
            next: (data) => {
              this.showAlert('Eliminación exitosa', 'Listo');
              this.showPersons();
            }, error: (e) => {
              this.showAlert('No se pudo eliminar', 'Error')
            }
          });
        }
      });
  }

  showAlert(message: string, action: string) {
    this._snackBar.open(message, action, {
      horizontalPosition: 'end',
      verticalPosition: 'top',
      duration: 3000,
    });
  }

  showPersons() {
    this._personService.getPerson().subscribe({
      next: (dataResponse) => {
        console.log(dataResponse);
        this.dataSource.data = dataResponse;
      },
      error: (e) => {},
    });
  }
}
