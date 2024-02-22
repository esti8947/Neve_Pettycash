import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ConfirmationService, MenuItem } from 'primeng/api';
import { Buyer } from 'src/app/models/buyer';
import { Department } from 'src/app/models/department';
import { EventCategory } from 'src/app/models/eventCategory';
import { ExpenseCategory } from 'src/app/models/expenseCategory';
import { AuthService } from 'src/app/services/auth-service/auth.service';
import { BuyerService } from 'src/app/services/buyer-service/buyer.service';
import { CustomMessageService } from 'src/app/services/customMessage-service/custom-message.service';
import { DepartmentService } from 'src/app/services/department-service/department.service';
import { EventCategoryService } from 'src/app/services/event-service/event-category.service';
import { ExpenseCategoryService } from 'src/app/services/expense-service/expense-category.service';



@Component({
  selector: 'home-manager',
  templateUrl: './home-manager.component.html',
  styleUrls: ['./home-manager.component.scss']
})
export class HomeManagerComponent implements OnInit {

  currentUser: any;
  departments: any[] = [];
  existingExpensesCategory: ExpenseCategory[] = [];
  existingEventsCategory: EventCategory[] = [];
  existingBuyers: Buyer[] = [];
  selectedExpenseCategory: any;
  selectedEventCategory: any;
  selectedBuyer: any;

  departmentDialog: boolean = false;
  expenseCategoryDialog: boolean = false;
  eventCategoryDialog: boolean = false;
  buyerDialog: boolean = false;

  departmentForm!: FormGroup;
  expenseCategoryForm!: FormGroup;
  eventCategoryForm!: FormGroup;
  buyerForm!: FormGroup;

  departmentFormSubmitted = false;
  expenseCategoryFormSubmitted = false;
  eventCategoryFormSubmitted = false;
  buyerFormSubmitted = false;


  constructor(
    private authService: AuthService,
    private departmentService: DepartmentService,
    private expenseCategoryService: ExpenseCategoryService,
    private eventCategoryService: EventCategoryService,
    private buyerService: BuyerService,
    private router: Router,
    private formBuilder: FormBuilder,
    private customMessageService: CustomMessageService,
    private confirmationService: ConfirmationService,
    private translateService: TranslateService,
  ) { }

  ngOnInit(): void {
    this.currentUser = this.authService.getCurrentUser();
    this.loadDepartments();
    this.loadExpensesCategory();
    this.loadEventCategory();
    this.loadBuyers();
    this.initializeForms();
  }

  initializeForms() {
    this.departmentForm = this.formBuilder.group({
      departmentName: ['', Validators.required],
      departmentCode: ['', Validators.required],
      deptHeadFirstName: ['', Validators.required],
      deptHeadLastName: ['', Validators.required],
      phonePerfix: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(3)]],
      phoneNumber: ['', [Validators.required, Validators.pattern(/^\d{7}$/)]],
      descreption: [''],
    });

    this.expenseCategoryForm = this.formBuilder.group({
      expenseCategoryType: ['', Validators.required],
      expenseCategoryName: ['', Validators.required],
      expenseCategoryNameHeb: ['', Validators.required],
      accountingCode: ['', Validators.required],
    });

    this.eventCategoryForm = this.formBuilder.group({
      eventCategoryName: ['', Validators.required],
      eventCategoryNameHeb: ['', Validators.required],
    });

    this.buyerForm = this.formBuilder.group({
      buyerName: ['', Validators.required],
      buyerNameHeb: ['', Validators.required],
    });
  }

  openAddDepartmentDialog() {
    this.departmentDialog = true;
  }

  openAddExpenseCategoryDialog() {
    this.expenseCategoryDialog = true;
  }
  openAddEventCategoryDialog() {
    this.eventCategoryDialog = true;
  }
  openAddBuyerDialog() {
    this.buyerDialog = true;
  }

  isInvalid(controlName: string, formGroup: FormGroup, formSubmitted: boolean): boolean {
    const control: AbstractControl | null = formGroup.get(controlName);
    return control ? (control.touched || formSubmitted) && control.invalid : false;
  }


  saveDepartment() {
    this.departmentFormSubmitted = true;
    if (this.departmentForm.valid) {
      const formValues = this.departmentForm.value;

      const newDepartment: Department = {
        departmentId: 0,
        departmentCode: formValues.departmentCode,
        departmentName: formValues.departmentName,
        deptHeadFirstName: formValues.deptHeadFirstName,
        deptHeadLastName: formValues.deptHeadLastName,
        phonePrefix: formValues.phonePerfix,
        phoneNumber: formValues.phoneNumber,
        description: formValues.descreption,
        isCurrent: true,
        currentBudgetTypeId: 1
      };
      console.log(newDepartment);

      this.departmentService.addDepartment(newDepartment).subscribe(
        () => {
          this.customMessageService.showSuccessMessage('Department is added');
          this.departmentForm.reset();
          this.departmentDialog = false;
          this.loadDepartments();
        },
        (error) => {
          console.error('Error saving department:', error);
          this.customMessageService.showErrorMessage('An error occurred while adding the department');
        }
      );
    }
  }

  saveExpenseCategory() {
    if (this.expenseCategoryForm.valid) {
      const formValues = this.expenseCategoryForm.value;

      const newExpenseCategory: ExpenseCategory = {
        expenseCategoryId: 0,
        expenseCategoryType: formValues.expenseCategoryType,
        expenseCategoryName: formValues.expenseCategoryName,
        expenseCategoryNameHeb: formValues.expenseCategoryNameHeb,
        accountingCode: formValues.accountingCode,
        isActive: true
      };

      this.expenseCategoryService.addExpenseCategory(newExpenseCategory).subscribe(
        () => {
          this.customMessageService.showSuccessMessage('expense category is added');
          this.expenseCategoryForm.reset();
          this.expenseCategoryFormSubmitted = false;
          this.loadExpensesCategory();
        },
        (error) => {
          console.error('Error saving expense:', error);
          this.customMessageService.showErrorMessage('An error occurred while adding the expense');
        }
      );
    }
  }

  saveEventCategory() {
    this.eventCategoryFormSubmitted = true;
    if (this.eventCategoryForm.valid) {
      const formValues = this.eventCategoryForm.value;

      const newEventCategory: EventCategory = {
        eventCategoryId: 0,
        eventCategoryName: formValues.eventCategoryName,
        eventCategoryNameHeb: formValues.eventCategoryNameHeb,
        isActive: true
      };
      this.eventCategoryService.addEventCategory(newEventCategory).subscribe(
        () => {
          this.customMessageService.showSuccessMessage('enent category is added');
          this.eventCategoryForm.reset();
          this.eventCategoryFormSubmitted = false;
          this.loadEventCategory();
        },
        (error) => {
          console.error('Error saving event:', error);
          this.customMessageService.showErrorMessage('An error occurred while adding the event');
        }
      );
    }
  }

  saveBuyer() {
    this.buyerFormSubmitted = true;
    if (this.buyerForm.valid) {
      const formValues = this.buyerForm.value;

      const newBuyer: Buyer = {
        buyerId: 0,
        buyerName: formValues.buyerName,
        buyerNameHeb: formValues.buyerNameHeb,
      };
      this.buyerService.addBuyer(newBuyer).subscribe(
        () => {
          this.customMessageService.showSuccessMessage('buyer is added');
          this.buyerForm.reset();
          this.buyerFormSubmitted = false;
          this.loadBuyers();
        },
        (error) => {
          console.error('Error saving buyer:', error);
          this.customMessageService.showErrorMessage('An error occurred while adding the buyer');
        }
      );
    }
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

  loadExpensesCategory() {
    this.expenseCategoryService.getAllExpenseCategories().subscribe(
      (data) => {
        this.existingExpensesCategory = data.data;
        console.log(this.existingExpensesCategory);
      },
      (error) => {
        console.error('An error occurred:', error);
      },
    );
  }

  loadEventCategory() {
    this.eventCategoryService.getEventsCategories().subscribe(
      (data) => {
        this.existingEventsCategory = data.data;
      },
      (error) => {
        console.error("An error accurred", error);
      }
    )
  }

  loadBuyers() {
    this.buyerService.getBuyers().subscribe(
      (data) => {
        this.existingBuyers = data.data;
      },
      (error) => {
        console.error("An error accurred", error);
      }
    )
  }

  getExpenseCategoryOptionLabel() {
    return this.translateService.currentLang === 'en-US' ? 'expenseCategoryName' : 'expenseCategoryNameHeb';
  }
  getEventCategoryOptionLabel() {
    return this.translateService.currentLang === 'en-US' ? 'eventCategoryName' : 'eventCategoryNameHeb';
  }
  getBuyerOptionLabel() {
    return this.translateService.currentLang === 'en-US' ? 'buyerName' : 'buyerNameHeb';
  }

  deleteExpenseCategory(event: Event, expenseCategory: any) {
    const expenseCategoryIdToDelete = expenseCategory.expenseCategoryId;
    this.confirmationService.confirm({
      target: event.target as EventTarget,
      message: 'Do you want to delete this expense category?',
      icon: 'pi pi-info-circle',
      acceptButtonStyleClass: ' p-button-sm',
      accept: () => {
        this.expenseCategoryService.deleteExpenseCategory(expenseCategoryIdToDelete).subscribe(
          (data) => {
            console.log('expense category is deleted', data);
            this.customMessageService.showSuccessMessage('Expense category is deleted');
            // this.existingExpensesCategory = this.existingExpensesCategory.filter((val) => val.expenseCategory.expenseCategoryId !== expenseCategoryIdToDelete);
            this.loadExpensesCategory();
          },
          (error) => {
            console.error('An error occurred:', error);
            this.customMessageService.showErrorMessage('An error occurred while deleting the expense category');
          }
        );
      },
    });
  }

  deleteEventCategory(event: Event, eventCategory: any) {
    const eventCategoryIdToDelete = eventCategory.eventCategoryId;
    this.confirmationService.confirm({
      target: event.target as EventTarget,
      message: 'Do you want to delete this event category?',
      icon: 'pi pi-info-circle',
      acceptButtonStyleClass: ' p-button-sm',
      accept: () => {
        this.eventCategoryService.deleteEventCategory(eventCategoryIdToDelete).subscribe(
          (data) => {
            console.log('event category is deleted', data);
            this.customMessageService.showSuccessMessage('Event category is deleted');
            // this.existingEventsCategory = this.existingEventsCategory.filter((val) => val.eventCategory.eventCategoryId !== eventCategoryIdToDelete);
            this.loadEventCategory();
          },
          (error) => {
            console.error('An error occurred:', error);
            this.customMessageService.showErrorMessage('An error occurred while deleting the event category');
          }
        );
      },
    });
  }

  deleteBuyer(event: Event, buyer: any) {
    const buyerIdToDelete = buyer.buyerId;
    this.confirmationService.confirm({
      target: event.target as EventTarget,
      message: 'Do you want to delete this buyer category?',
      icon: 'pi pi-info-circle',
      acceptButtonStyleClass: 'p-button-sm',
      accept: () => {
        this.buyerService.deleteBuyer(buyerIdToDelete).subscribe(
          (data) => {
            console.log('buyer category is deleted', data);
            this.customMessageService.showSuccessMessage('Buyer category is deleted');
            this.loadBuyers();
          },
          (error) => {
            console.error('An error occurred:', error);
            this.customMessageService.showErrorMessage('An error occurred while deleting the buyer category');
          }
        );
      },
    });
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
