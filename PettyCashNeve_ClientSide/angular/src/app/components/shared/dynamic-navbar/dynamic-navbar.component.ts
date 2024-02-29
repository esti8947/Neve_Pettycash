import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { MenuItem } from 'primeng/api';
import { AnnualBudget } from 'src/app/models/annualBudget';
import { MonthlyBudget } from 'src/app/models/monthlyBudget';
import { NewYear } from 'src/app/models/newYear';
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
  months:any[]=[];
  currentUser: any;
  selectedDepartment: any;

  addAmountForm!:FormGroup;
  newYearFormGroup!:FormGroup;
  // validForm: boolean = true;
  addingAmountDialog:boolean = false;
  newYearDialog:boolean = false;

  addAmountFormSubmitted = false;
  newYearFromSubmitted = false;

  budgetTypeId:number = 0;

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
      monthlyBudgetAmount: [''],
      MonthlyBudgetMonth:['']
    });
  }

  isInvalid(controlName: string, formGroup: FormGroup, formSubmitted: boolean): boolean {
    const control: AbstractControl | null = formGroup.get(controlName);
    return control ? (control.touched || formSubmitted) && control.invalid : false;
  }

  loadMonths() {
    const currentDate = new Date();
    const currentMonth = currentDate.getMonth();
  
    const monthNames = [
      'January', 'February', 'March', 'April', 'May', 'June',
      'July', 'August', 'September', 'October', 'November', 'December'
    ];
  
    this.months = Array.from({ length: 12 }, (_, index) => ({
      value: index + 1, // Months are 1-based in JavaScript Date object
      name: monthNames[index]
    }));
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

  openNewYear() {
    this.newYearFromSubmitted = true;
    if (this.newYearFormGroup.valid) {
      let { newYear, budgetType, annualBudgetAmount, monthlyBudgetAmount, MonthlyBudgetMonth} = this.newYearFormGroup.value;
  
      // Format newYear to remove the dash
      newYear = newYear.replace('-', '');
      const departmentId = this.selectedDepartment.departmentId;

      const annualBudgetModel:AnnualBudget={
        id: 0,
        currentYear: newYear,
        annualBudgetCeiling: annualBudgetAmount,
        isActive: true,
        departmentId: departmentId,
      }
      const monthlyBudgetModel:MonthlyBudget = {
        id: 0,
        currentYear: newYear,
        isActive: true,
        departmentId: departmentId,
        monthNumber: MonthlyBudgetMonth.value,
        monthlyBudgetCeiling: monthlyBudgetAmount,
      }

      const newYearModel:NewYear ={
        newYear: newYear,
        departmentId: departmentId,
        budgetTypeId: budgetType.budgetTypeId,
        annaulBudget: annualBudgetModel,
        monthlyBudget: monthlyBudgetModel
      }
  
      this.additionalActionsService.openNewYear(newYearModel).subscribe(
        () => {
          // Handle success if needed
        },
        (error) => {
          console.error('An error occurred:', error);
          // Handle error if needed
        }
      );
    }
  }

  onBudgetTypeChange(selectedBudgetTypeId: number) {
    this.budgetTypeId = selectedBudgetTypeId;
    if (selectedBudgetTypeId === 1) {
      // Show input for annual budget amount
      this.newYearFormGroup.get('annualBudgetAmount')?.setValidators(Validators.required);
      this.newYearFormGroup.get('annualBudgetAmount')?.updateValueAndValidity();

      // Hide input for monthly budget amount
      this.newYearFormGroup.get('monthlyBudgetAmount')?.clearValidators();
      this.newYearFormGroup.get('monthlyBudgetAmount')?.updateValueAndValidity();
      this.newYearFormGroup.get('MonthlyBudgetMonth')?.clearValidators();
      this.newYearFormGroup.get('MonthlyBudgetMonth')?.updateValueAndValidity();
    } else if (selectedBudgetTypeId === 2) {
      this.loadMonths();
      // Show input for monthly budget amount
      this.newYearFormGroup.get('monthlyBudgetAmount')?.setValidators(Validators.required);
      this.newYearFormGroup.get('monthlyBudgetAmount')?.updateValueAndValidity();
      this.newYearFormGroup.get('MonthlyBudgetMonth')?.setValidators(Validators.required);
      this.newYearFormGroup.get('MonthlyBudgetMonth')?.updateValueAndValidity();


      // Hide input for annual budget amount
      this.newYearFormGroup.get('annualBudgetAmount')?.clearValidators();
      this.newYearFormGroup.get('annualBudgetAmount')?.updateValueAndValidity();
    }
  }
}
