import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { AuthService } from 'src/app/services/auth-service/auth.service';
import { DepartmentService } from 'src/app/services/department-service/department.service';



@Component({
  selector: 'home-manager',
  templateUrl: './home-manager.component.html',
  styleUrls: ['./home-manager.component.scss']
})
export class HomeManagerComponent implements OnInit {

  currentUser:any;
  departments: any[] = [];
  departmentDialog:boolean = false;
  expenseCategoryDialog:boolean = false;
  eventCategoryDialog:boolean = false;
  buyerDialog:boolean = false;
  departmentForm!: FormGroup;
  expenseCategoryForm!: FormGroup;
  eventCategoryFrom!:FormGroup;
  buyerFrom!:FormGroup;


  constructor(
    private authService:AuthService,
    private departmentService:DepartmentService,
    private router: Router,
    private fb: FormBuilder
  ) { }

  ngOnInit(): void {
    this.currentUser = this.authService.getCurrentUser();
    this.loadDepartments();
    this.initializeDepartmentForm();
    this.initializeExpenseCategoryForm();
    this.initializeEventCategoryForm();
    this.initializeBuyerForm();
  }
  initializeExpenseCategoryForm() {
    throw new Error('Method not implemented.');
  }
  initializeBuyerForm() {
    throw new Error('Method not implemented.');
  }
  initializeEventCategoryForm() {
    throw new Error('Method not implemented.');
  }
  initializeDepartmentForm() {
    throw new Error('Method not implemented.');
  }

  openAddDepartmentDialog(){
    this.departmentDialog = true;
  }

  openAddExpenseCategoryDialog(){
    this.expenseCategoryDialog = true;
  }
  openAddEventCategoryDialog(){
    this.eventCategoryDialog = true;
  }
  openAddBuyerDialog(){
    this.buyerDialog = true;
  }

  loadDepartments() {
    this.departmentService.getAllDepartments().subscribe(
      (data) => {
        this.departments = data.data;
        console.log(this.departments)
      },
      (error) => {
        console.error('An error occurred', error)
      }
      )
    }

    selectDepartment(selectedDepartment: any) {
      if (selectedDepartment) {
        this.departmentService.saveSelectedDepartmentToLocalStorage(selectedDepartment);
  
        this.authService.updateCurrentUserDepartmentId(selectedDepartment.departmentId);
        this.router.navigate(['/navbar']);
      }
    }

    logOut() {
      this.authService.doLogout();
      this.loadDepartments();
    }
  }
