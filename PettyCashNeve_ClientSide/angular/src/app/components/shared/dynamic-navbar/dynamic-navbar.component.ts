import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { MenuItem } from 'primeng/api';
import { AdditionalActionsService } from 'src/app/services/additional-actions-service/additional-actions.service';
import { AuthService } from 'src/app/services/auth-service/auth.service';
import { BudgetTypeService } from 'src/app/services/budgetType-service/budget-type.service';
import { CustomMessageService } from 'src/app/services/customMessage-service/custom-message.service';
import { DepartmentService } from 'src/app/services/department-service/department.service';

interface CustomMenuItem extends MenuItem {
  route: string;
}

@Component({
  selector: 'app-dynamic-navbar',
  templateUrl: './dynamic-navbar.component.html',
  styleUrls: ['./dynamic-navbar.component.scss']
})
export class DynamicNavbarComponent implements OnInit {
  navbarItems: CustomMenuItem[] = [];
  budgetTypes:any[] = [];
  currentUser: any;
  selectedDepartment: any;

  addAmountForm!:FormGroup;
  newYearFormGroup!:FormGroup;
  // validForm: boolean = true;
  addingAmountDialog:boolean = false;
  newYearDialog:boolean = false;

  addAmountFormSubmitted = false;
  newYearFromSubmitted = false;

  constructor(
    private authService: AuthService,
    private departmentService: DepartmentService,
    private budgetTypeService:BudgetTypeService,
    private router: Router,
    private formBuilder: FormBuilder,
    private additionalActionsService: AdditionalActionsService,
    private customMessageService:CustomMessageService,
    private translateService:TranslateService,

  ) { }

  ngOnInit(): void {
    this.currentUser = this.authService.getCurrentUser();
    this.selectedDepartment = this.departmentService.getSelectedDepartment();
    this.initializeNavbarItems();
    this.initializeForms();

  }

  initializeNavbarItems() {
    if (this.currentUser?.isManager) {
      this.navbarItems = [
        { label: 'navbar.Home', route: '/navbar/home-department' },
        { label: 'navbar.Expense Details', route: '/navbar/expense-report' },
        { label: 'navbar.View previous months', route: '/navbar/previous-expenses' },
      ];
    } else {
      this.navbarItems = [
        { label: 'navbar.Home', route: '/navbar/home-department' },
        { label: 'navbar.Inserting a new expense', route: '/navbar/add-expense' },
        { label: 'navbar.Inserting a new activity', route: '/navbar/add-event' },
        { label: 'navbar.Expense Details', route: '/navbar/expense-report' },
        { label: 'navbar.View previous months', route: '/navbar/previous-expenses' },
      ];
    }
  }

  isActiveRoute(route: string): boolean {
    return this.router.isActive(route, true);
  }

  logOut() {
    if (this.currentUser.isManager) {
      this.departmentService.deactivateSelectedDepartment();
      this.router.navigate(['/home-manager']);
    } else {
      this.authService.doLogout();
    }
  }

  initializeForms(){
    this.addAmountForm = this.formBuilder.group({
      amountToAdd: new FormControl<number | null>(null, Validators.required)      
    });

    this.newYearFormGroup = this.formBuilder.group({
      newYear: ['', [Validators.required, Validators.pattern(/^\d{4}-\d{4}$/)]], // 8 digits pattern (e.g., 20232024)
      budgetType: ['', Validators.required],
      annualBudgetAmount: [''],
      monthlyBudgetAmount: ['']
    });
  }

  isInvalid(controlName: string, formGroup: FormGroup, formSubmitted: boolean): boolean {
    const control: AbstractControl | null = formGroup.get(controlName);
    return control ? (control.touched || formSubmitted) && control.invalid : false;
  }

  openAddingAmountDialog() {
    this.addingAmountDialog = true;
  }

  addingAmountToBudget(){
    this.addAmountFormSubmitted = true;

    if (this.addAmountForm.valid) {
      const amoutnToAdd = this.addAmountForm.value.amountToAdd;
      const departmentId = this.selectedDepartment.departmentId;
      this.additionalActionsService.addAmountToBudget(departmentId, amoutnToAdd).subscribe(
        (response) =>{
          console.log('insert refund amount succeddfull: ', response);
          this.addAmountForm.reset();
          this.addAmountFormSubmitted = false;
          this.addingAmountDialog = false;
          this.router.navigate(['navbar']);
          this.customMessageService.showSuccessMessage("add amount to budget is successfull.");
        },
        (error)=>{
          console.error('An error occurred while add amount to budget: ', error);
          this.customMessageService.showErrorMessage("'An error occurred while add amount");
        }
      )
    }
  }

  getOptionLabel(){
    return this.translateService.currentLang === 'en-US'? 'budgetTypeName':'budgetTypeNameHeb';
  }

  openNewYearDialog(){
    this.budgetTypeService.getBudgetTypes().subscribe(
      (data) => {
        this.budgetTypes = data.data;
        console.log("eventcategories",this.budgetTypes);
      },
      (error) => {
        console.error('An error occurred:', error);
      },
    );
    this.newYearDialog = true;
  }

  openNewYear(){
    this.newYearFromSubmitted = true;
    if(this.newYearFormGroup.valid){

    }
  }
}
