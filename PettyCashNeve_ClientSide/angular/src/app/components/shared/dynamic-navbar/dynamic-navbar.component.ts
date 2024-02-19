import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { AdditionalActionsService } from 'src/app/services/additional-actions-service/additional-actions.service';
import { AuthService } from 'src/app/services/auth-service/auth.service';
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
  currentUser: any;
  selectedDepartment: any;

  formGroup!: FormGroup;
  validForm: boolean = true;
  addingAmountDialog:boolean = false;

  constructor(
    private authService: AuthService,
    private departmentService: DepartmentService,
    private router: Router,

    private formBuilder: FormBuilder,
    private additionalActionsService: AdditionalActionsService,
    private customMessageService:CustomMessageService,

  ) { }

  ngOnInit(): void {
    this.currentUser = this.authService.getCurrentUser();
    this.selectedDepartment = this.departmentService.getSelectedDepartment();
    this.initializeNavbarItems();
    this.initializeForm();

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
  initializeForm() {
    this.formGroup = this.formBuilder.group({
      amountToAdd: new FormControl<number | null>(null, Validators.required)
    });
  }

  openAddingAmountDialog() {
    this.addingAmountDialog = true;
  }

  addingAmountToBudget(){
    this.validForm = !this.formGroup.invalid;
    if (this.validForm) {
      const amoutnToAdd = this.formGroup.value.amountToAdd;
      const departmentId = this.selectedDepartment.departmentId;
      this.additionalActionsService.addAmountToBudget(departmentId, amoutnToAdd).subscribe(
        (response) =>{
          console.log('insert refund amount succeddfull: ', response);
          this.formGroup.reset();
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
}
