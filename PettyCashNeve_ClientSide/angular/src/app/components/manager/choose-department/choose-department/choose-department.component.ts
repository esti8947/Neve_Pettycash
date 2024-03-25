import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth-service/auth.service';
import { DepartmentService } from 'src/app/services/department-service/department.service';

@Component({
  selector: 'choose-department',
  templateUrl: './choose-department.component.html',
  styleUrls: ['./choose-department.component.scss']
})
export class ChooseDepartmentComponent implements OnInit {
  departments: any[] = [];
  formGroup!: FormGroup;

  constructor(
    private departmentService: DepartmentService,
    private router: Router,
    private authService:AuthService,
  ) {}

  ngOnInit(): void {
    this.loadDepartments();
    this.initializeForm();
  }

  loadDepartments() {
    this.departmentService.getAllDepartments().subscribe(
      (data) => {
        this.departments = data.data;
      },
      (error) => {
        console.error('An error occurred', error)
      }
    )
  }

  initializeForm() {
    this.formGroup = new FormGroup({
      selectedDepartment: new FormControl(null),
    });
  }

  onDropdownChange() {
    const selectedDepartment = this.formGroup.get('selectedDepartment')?.value;

    if (selectedDepartment) {
      this.departmentService.saveSelectedDepartmentToLocalStorage(selectedDepartment);
      this.authService.updateCurrentUserDepartmentId(selectedDepartment.departmentId);
      this.router.navigate(['/navbar']);
    }
  }
}
