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
import { MonthlyBudgetService } from 'src/app/services/monthlyBudget-service/monthly-budget.service';

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
  budgetTypes: any[] = [];
  months: any[] = [];
  currentUser: any;
  selectedDepartment: any;

  addAmountForm!: FormGroup;
  newYearFormGroup!: FormGroup;
  addMonthlyBudgetForm!: FormGroup;
  // validForm: boolean = true;
  addingAmountDialog: boolean = false;
  newYearDialog: boolean = false;
  AddMothnlyBudgetDialog: boolean = false;

  addAmountFormSubmitted: boolean = false;
  newYearFromSubmitted: boolean = false;
  addMonthlyBudgetSubmitted: boolean = false;

  budgetTypeId: number = 0;

  constructor(
    private authService: AuthService,
    private departmentService: DepartmentService,
    private budgetTypeService: BudgetTypeService,
    private monthlyBudgetService: MonthlyBudgetService,
    private router: Router,
    private formBuilder: FormBuilder,
    private additionalActionsService: AdditionalActionsService,
    private customMessageService: CustomMessageService,
    private translateService: TranslateService,

  ) { }

  ngOnInit(): void {
    this.currentUser = this.authService.getCurrentUser();
    this.selectedDepartment = this.departmentService.getSelectedDepartment();
    this.initializeNavbarItems();
    this.initializeForms();
    this.loadMonths();
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

  initializeForms() {
    this.addAmountForm = this.formBuilder.group({
      amountToAdd: new FormControl<number | null>(null, Validators.required)
    });

    this.newYearFormGroup = this.formBuilder.group({
      newYear: new FormControl<number | null>(null, [Validators.required, Validators.pattern(/^\d{4}-\d{4}$/)]), // 8 digits pattern (e.g., 20232024)
      budgetType: new FormControl<number | null>(null, Validators.required),
      annualBudgetAmount: new FormControl<number | null>(null, Validators.required),
      monthlyBudgetAmount: new FormControl<number | null>(null, Validators.required),
      MonthlyBudgetMonth: new FormControl<number | null>(null, Validators.required)
    });

    this.addMonthlyBudgetForm = this.formBuilder.group({
      monthlyBudgetYear: new FormControl<number | null>(null, [Validators.required, Validators.pattern(/^\d{4}-\d{4}$/)]), // 8 digits pattern (e.g., 20232024)
      monthlyBudgetAmount: new FormControl<number | null>(null, Validators.required),
      MonthlyBudgetMonth: new FormControl<number | null>(null, Validators.required)
    })
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

  openAddMothnlyBudgetDialog() {
    this.AddMothnlyBudgetDialog = true;
  }

  addingAmountToBudget() {
    this.addAmountFormSubmitted = true;

    if (this.addAmountForm.valid) {
      const amoutnToAdd = this.addAmountForm.value.amountToAdd;
      const departmentId = this.selectedDepartment.departmentId;
      this.additionalActionsService.addAmountToBudget(departmentId, amoutnToAdd).subscribe(
        (response) => {
          console.log('insert refund amount succeddfull: ', response);
          this.addAmountForm.reset();
          this.addAmountFormSubmitted = false;
          this.addingAmountDialog = false;
          this.router.navigate(['navbar']);
          this.customMessageService.showSuccessMessage("add amount to budget is successfull.");
        },
        (error) => {
          console.error('An error occurred while add amount to budget: ', error);
          this.customMessageService.showErrorMessage("'An error occurred while add amount");
        }
      )
    }
  }

  getOptionLabel() {
    return this.translateService.currentLang === 'en-US' ? 'budgetTypeName' : 'budgetTypeNameHeb';
  }

  openNewYearDialog() {
    this.budgetTypeService.getBudgetTypes().subscribe(
      (data) => {
        this.budgetTypes = data.data;
        console.log("eventcategories", this.budgetTypes);
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
      let { newYear, budgetType, annualBudgetAmount, monthlyBudgetAmount, MonthlyBudgetMonth } = this.newYearFormGroup.value;

      // Format newYear to remove the dash
      newYear = newYear.replace('-', '');
      const departmentId = this.selectedDepartment.departmentId;

      const annualBudgetModel: AnnualBudget = {
        annualBudgetId: 0,
        annualBudgetYear: newYear,
        annualBudgetCeiling: annualBudgetAmount,
        isActive: true,
        departmentId: departmentId,
      }
      const monthlyBudgetModel: MonthlyBudget = {
        monthlyBudgetId: 0,
        monthlyBudgetYear: newYear,
        isActive: true,
        departmentId: departmentId,
        monthlyBudgetMonth: MonthlyBudgetMonth.value,
        monthlyBudgetCeiling: monthlyBudgetAmount,
      };

      const newYearModel: NewYear = {
        newYear: newYear,
        departmentId: departmentId,
        budgetTypeId: budgetType.budgetTypeId,
        annualBudget: annualBudgetModel,
        monthlyBudget: monthlyBudgetModel
      };
     
      this.additionalActionsService.openNewYear(newYearModel).subscribe(
        (response) => {
          console.log("open new year is successfull", response);
          this.customMessageService.showSuccessMessage('new year is open');
          this.newYearFormGroup.reset();
          this.newYearFromSubmitted = false;
          this.newYearDialog = false;
          this.router.navigate(['navbar']);
        },
        (error) => {
          console.error('An error occurred while add the expense:', error);
          this.customMessageService.showErrorMessage('An error occurred while adding the expense');
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

  addMonthlyBudget() {
    this.addAmountFormSubmitted = true;
    if (this.addMonthlyBudgetForm.valid) {

      let { monthlyBudgetYear, monthlyBudgetAmount, MonthlyBudgetMonth } = this.addMonthlyBudgetForm.value;
      monthlyBudgetYear = monthlyBudgetYear.replace('-', '');

      const monthlyBudget: MonthlyBudget = {
        monthlyBudgetId: 0,
        monthlyBudgetYear: monthlyBudgetYear,
        isActive: true,
        departmentId: this.selectedDepartment.departmentId,
        monthlyBudgetMonth: MonthlyBudgetMonth.value,
        monthlyBudgetCeiling: monthlyBudgetAmount
      }
      this.monthlyBudgetService.createMonthlyBudget(monthlyBudget).subscribe(
        (response) => {
          this.addMonthlyBudgetForm.reset();
          this.addMonthlyBudgetSubmitted = false;
          this.AddMothnlyBudgetDialog = false;
          this.router.navigate(['navbar']);
          this.customMessageService.showSuccessMessage("adding monthly budget is successfull.");
        },
        (error) => {
          console.error('An error occurred while adding monthly budget: ', error);
          this.customMessageService.showErrorMessage("'An error occurred while adding monthly budget");
        }
      );
    }
  }
}