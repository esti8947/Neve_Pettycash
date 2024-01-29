import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { BudgetTypeService } from 'src/app/services/budgetType-service/budget-type.service';
import { DepartmentService } from 'src/app/services/department-service/department.service';

@Component({
  selector: 'add-department',
  templateUrl: './add-department.component.html',
  styleUrls: ['./add-department.component.scss']
})
export class AddDepartmentComponent implements OnInit{
  departmentDialog: boolean = false;
  formGroup!: FormGroup;
  validForm: boolean = true;
  budgetTypeList: any[] = [];

  constructor(
    private formBuilder:FormBuilder,
    private budgetTypeService: BudgetTypeService,
    private departmentService:DepartmentService) {}

  ngOnInit(){
    this.initializeForm();
    this.loadBudgetType();
  }

  loadBudgetType(){
    this.budgetTypeService.getBudgetTypes().subscribe(
      (data) => {
        this.budgetTypeList = data.data;
      },
      (error) => {
        this.budgetTypeList = [
          { id: 1, name: "annual budget" },
          { id: 2, name: "monthly budget" }
        ];
        console.error('An error occurred:', error);
      }
    );
  }

  isInvalid(controlName: string): boolean {
    const control: AbstractControl | null = this.formGroup.get(controlName);
    return control ? control.touched && control.invalid : false;
  }

  initializeForm() {
    this.formGroup = this.formBuilder.group({
      departmentName: new FormControl<string | null>(null, Validators.required),
      departmentCode: new FormControl<string | null>(null, Validators.required),
      deptHeadFirstName: new FormControl<string | null>(null, Validators.required),
      deptHeadLastName: new FormControl<string | null>(null, Validators.required),
      phonePerfix: new FormControl<string | null>(null, Validators.required),
      phoneNumber: new FormControl<string | null>(null, Validators.required),
      descreption: new FormControl<string | null>(null),
      budgetTypeId: new FormControl<number | null>(null, Validators.required),
    });
  }

  addDepartment(){
    this.departmentDialog = true;
  }

  async saveDepartment(){
    if(this.validForm){
      const newDepartment = {

      }

      try{
      }catch(error){

      }
    }
  }
}
