import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ConfirmationService } from 'primeng/api';
import { RegisterUser } from 'src/app/models/registerUser';
import { AdditionalActionsService } from 'src/app/services/additional-actions-service/additional-actions.service';
import { AuthService } from 'src/app/services/auth-service/auth.service';
import { CustomMessageService } from 'src/app/services/customMessage-service/custom-message.service';
import { DepartmentService } from 'src/app/services/department-service/department.service';

@Component({
  selector: 'app-users-of-department',
  templateUrl: './users-of-department.component.html',
  styleUrls: ['./users-of-department.component.scss']
})
export class UsersOfDepartmentComponent implements OnInit{
  usersList:any[] = [];
  selectedDepartment:any;
  visible: boolean = false;
  formGroup!: FormGroup;
  validForm: boolean = true;

  constructor(
    private additionalActionsService:AdditionalActionsService,
    private departmentService:DepartmentService,
    private authService:AuthService,
    private formBuilder: FormBuilder,
    private customMessageService:CustomMessageService,
    private confirmationService:ConfirmationService,){}

  ngOnInit(): void {
    this.selectedDepartment = this.departmentService.getSelectedDepartment();
    this.loadUsers();
    this.initializeForm();
  }

  loadUsers(){
    this.additionalActionsService.getUsersOfDepartment(this.selectedDepartment.departmentId).subscribe(
      (data) =>{
        this.usersList = data.data;
        console.log(this.usersList)
      },
      (error)=>{
        console.error('An error occurred:', error);        
      }
    )
  }

  showDialog() {
      this.visible = true;
  }

  initializeForm() {
    this.formGroup = this.formBuilder.group({
      username: new FormControl<string>('',Validators.required),
      password: new FormControl<string>('', Validators.required),
      email: new FormControl<string>('', Validators.required),
      phoneNumber: new FormControl<string|null>(null),
    });
  }

  registerUser(){
    this.validForm = !this.formGroup.invalid;
    if(this.validForm){
      const{username, password, email, phoneNumber} = this.formGroup.value;
      const departmentId = this.selectedDepartment.departmentId;
      const newUser:RegisterUser = {
        username: username,
        password: password,
        email: email,
        phoneNumber: phoneNumber,
        departmentId: departmentId,
        isAdmin: false
      };

      this.authService.registerUser(newUser).subscribe(
        (response) => {
          console.log('user added successfully:', response);
          this.visible = false;
          this.customMessageService.showSuccessMessage('User register successfully')
          this.formGroup.reset();
          this.loadUsers();
        },
        (error) => {
          console.error('An error occurred while adding the event:', error);
          this.customMessageService.showErrorMessage('An error occurred while adding the event')
        }
      )
    }
  }
}
