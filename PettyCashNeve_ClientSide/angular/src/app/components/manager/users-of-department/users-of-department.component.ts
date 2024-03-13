import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { ConfirmationService } from 'primeng/api';
import { emailValidator, passwordValidator } from 'src/app/custom-validators';
import { RegisterModel } from 'src/app/models/registerUser';
import { AdditionalActionsService } from 'src/app/services/additional-actions-service/additional-actions.service';
import { AuthService } from 'src/app/services/auth-service/auth.service';
import { CustomMessageService } from 'src/app/services/customMessage-service/custom-message.service';
import { DepartmentService } from 'src/app/services/department-service/department.service';

@Component({
  selector: 'app-users-of-department',
  templateUrl: './users-of-department.component.html',
  styleUrls: ['./users-of-department.component.scss']
})
export class UsersOfDepartmentComponent implements OnInit {
  usersList: any[] = [];
  selectedDepartment: any;
  registerUserDialog: boolean = false;
  registerUserForm!: FormGroup;
  updateUserForm!: FormGroup;

  registerFormFormSubmitted = false;
  updateFormFormSubmitted = false;

  // validForm: boolean = true;
  user: any;
  editUserDialog: boolean = false;
  selectedUserId: string = "";

  constructor(
    private additionalActionsService: AdditionalActionsService,
    private departmentService: DepartmentService,
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private customMessageService: CustomMessageService,
    private confirmationService: ConfirmationService,
    private translateService: TranslateService,) { }

  ngOnInit(): void {
    this.selectedDepartment = this.departmentService.getSelectedDepartment();
    this.loadUsers();
    this.initializeForms();
  }

  loadUsers() {
    this.additionalActionsService.getUsersOfDepartment(this.selectedDepartment.departmentId).subscribe(
      (data) => {
        this.usersList = data.data;
        console.log(this.usersList)
      },
      (error) => {
        console.error('An error occurred:', error);
      }
    )
  }

  openRegiterUsersDialog() {
    this.registerUserDialog = true;
  }

  initializeForms() {
    this.registerUserForm = this.formBuilder.group({
      userName: new FormControl<string>('', Validators.required),
      passwordHash: new FormControl<string>('', [Validators.required, passwordValidator()]),
      email: new FormControl<string>('', [Validators.required, emailValidator()]),
      phoneNumber: new FormControl<string | null>(null, Validators.pattern('[0-9]{9,10}')),
    });

    this.updateUserForm = this.formBuilder.group({
      userId: new FormControl<string>('', Validators.required),
      userName: new FormControl<string>('', Validators.required),
      email: new FormControl<string>('', [Validators.required, emailValidator()]),
      phoneNumber: new FormControl<string | null>(null, Validators.pattern('[0-9]{9,10}')),
    })
  }

  isInvalid(controlName: string, formGroup: FormGroup, formSubmitted: boolean): boolean {
    const control: AbstractControl | null = formGroup.get(controlName);
    return control ? (control.touched || formSubmitted) && control.invalid : false;
  }

  registerUser() {
    if (this.registerUserForm.valid) {
      const formValues = this.registerUserForm.value;
      const departmentId = this.selectedDepartment.departmentId;
      const newUser: RegisterModel = {
        userName: formValues.userName,
        passwordHash: formValues.passwordHash,
        email: formValues.email,
        phoneNumber: formValues.phoneNumber || "",
        departmentId: departmentId,
        isAdmin: false 
      };

      this.authService.registerUser(newUser).subscribe(
        (response) => {
          console.log('user added successfully:', response);
          this.registerUserDialog = false;
          this.customMessageService.showSuccessMessage('User register successfully')
          this.registerUserForm.reset();
          this.registerFormFormSubmitted = false;
          this.loadUsers();
        },
        (error) => {
          console.error('An error occurred while adding the event:', error);
          this.customMessageService.showErrorMessage('An error occurred while adding the event')
        }
      )
    }
  }

  deleteUser(event: MouseEvent, user: any) {
    const usernameToDelete = user.username;
    this.confirmationService.confirm({
      target: event.target as EventTarget,
      message: this.translateService.instant('messages.deleteUserConfirmation'),
      icon: 'pi pi-info-circle',
      acceptButtonStyleClass: ' p-button-sm',
      accept: () => {
        this.authService.deactivateUser(usernameToDelete).subscribe(          
          (data) => {
            console.error('An error occurred:', data);
            this.customMessageService.showErrorMessage('An error occurred while deleting the user');
          },
          (error) => {
            console.log('user is deleted', error);
            this.customMessageService.showSuccessMessage(this.translateService.instant('messages.userDeleted'));
            this.loadUsers();
          }
        );
      },
    })
  }

  editUser(user: any) {
    console.log(user)
    this.user = user;
    this.editUserDialog = true;
    this.selectedUserId = user.id; 

    this.updateUserForm.patchValue({
      userId: this.selectedUserId,
      userName: user.username,
      email: user.email,
      phoneNumber: user.phoneNumber
    });
  }


  updateUser() {
    if (this.updateUserForm.valid) {
      const formValues = this.updateUserForm.value;
      const updatedUser: any = {
        id: this.selectedUserId,
        userName: formValues.userName,
        email: formValues.email,
        phoneNumber: formValues.phoneNumber || "", // Assuming phoneNumber is nullable
      };

      this.authService.updateUser(updatedUser).subscribe(
        (response) => {
          console.log('user updated successfully:', response);
          this.editUserDialog = false;
          this.customMessageService.showSuccessMessage('User update successfully')
          this.updateUserForm.reset();
          this.updateFormFormSubmitted = false;
          this.loadUsers();
        },
        (error) => {
          console.error('An error occurred while update the event:', error);
          this.customMessageService.showErrorMessage('An error occurred while update the event')
        }
      )
    }
  }
}
