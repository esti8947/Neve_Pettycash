import { Component, Input, OnInit } from '@angular/core';
import { ExpenseService } from 'src/app/services/expense-service/expense.service';
import { ConfirmationService } from 'primeng/api';
import { ExpenseReportInfoService } from 'src/app/services/expense-service/expense-report-info.service';
import { ExpenseCategory } from 'src/app/models/expenseCategory';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { ExpenseCategoryService } from 'src/app/services/expense-service/expense-category.service';
import { BuyerService } from 'src/app/services/buyer-service/buyer.service';
import { EventService } from 'src/app/services/event-service/event.service';
import { ActivatedRoute, Router } from '@angular/router';
import { NewExpenseModel } from 'src/app/models/expense';
import { CustomMessageService } from 'src/app/services/customMessage-service/custom-message.service';
import { MontlyCashRegisterService } from 'src/app/services/montlyCashRegister-service/montly-cash-register.service';
import { AuthService } from 'src/app/services/auth-service/auth.service';
import { TranslateService } from '@ngx-translate/core';
import { AdditionalActionsService } from 'src/app/services/additional-actions-service/additional-actions.service';

@Component({
  selector: 'expense-report',
  templateUrl: './expense-report.component.html',
  styleUrls: ['./expense-report.component.scss'],
})
export class ExpenseReportComponent implements OnInit {
  currentUser: any;
  expenses: any[] = [];
  expense: any;
  selectedExpense: any;
  expenseDialog: boolean = false;
  submitted: boolean = false;
  expensesCategory: ExpenseCategory[] = [];
  events: any[] = [];
  buyers: any[] = [];
  formGroup!: FormGroup;
  validForm: boolean = true;
  monthlyRegister: any;
  year: number | undefined;
  month: number | undefined;

  constructor(
    private translateService: TranslateService,
    private authService: AuthService,
    private expenseService: ExpenseService,
    private expenseReportInfoService: ExpenseReportInfoService,
    private customMessageService: CustomMessageService,
    private confirmationService: ConfirmationService,
    private expenseCategoryService: ExpenseCategoryService,
    private buyerService: BuyerService,
    private monthlyCashRegisterService: MontlyCashRegisterService,
    private eventService: EventService,
    private additionalActionsService:AdditionalActionsService,
    private route: ActivatedRoute,
    private router: Router,
  ) { }

  ngOnInit() {
    this.currentUser = this.authService.getCurrentUser();
    this.route.queryParams.subscribe(params => {
      this.year = params['selectedYear'];
      this.month = params['selectedMonth'];

      // Check if year and month are undefined
      if (this.year == undefined && this.month == undefined) {
        this.monthlyRegister = this.monthlyCashRegisterService.getCurrentMothlyRegister();
        if(this.monthlyRegister.length == 0 && this.currentUser.isManager){
          this.GetUnLockedExpensesByDepartmentId(this.currentUser.departmentId);
        }
        // Check if monthlyRegister is an array
        if (Array.isArray(this.monthlyRegister) && this.monthlyRegister.length > 0) {
          // Find the object with the earliest monthlyCashRegisterMonth
          const earliestRegister = this.monthlyRegister.reduce((earliest, current) =>
            earliest.monthlyCashRegisterMonth < current.monthlyCashRegisterMonth ? earliest : current);

          // Update year and month accordingly
          this.month = earliestRegister.monthlyCashRegisterMonth;
          this.year = earliestRegister.monthlyCashRegisterYear;
        } else if (!Array.isArray(this.monthlyRegister)) { // Check if monthlyRegister is not an array
          // If it's a single object, use its year and month
          this.month = this.monthlyRegister.monthlyCashRegisterMonth;
          this.year = this.monthlyRegister.monthlyCashRegisterYear;
        }
      }

      if (this.year && this.month) {
        if (this.currentUser.isManager) {
          const departmentId = this.currentUser.departmentId;
          this.loadExpensesByYearAndMonth(this.year, this.month, departmentId);
        }
        this.loadExpensesByYearAndMonth(this.year, this.month);
      }
    });
  }



  loadExpensesByYearAndMonth(selectedYear: number, selectedMonth: number, departmentId?: number) {
    this.expenseReportInfoService.getExpensesReportByYearAndMonth(selectedYear, selectedMonth, departmentId).subscribe(
      (data) => {
        this.expenses = data.data || [];
        console.log("previus expenses", this.expenses);
      },
      (error) => {
        console.log('An error occurred: ', error);
      },
    );
  }
  GetUnLockedExpensesByDepartmentId(departmentId:number){
    this.expenseReportInfoService.GetUnLockedExpensesByDepartmentId(departmentId).subscribe(
      (data) =>{
        this.expenses = data.data || [];
        console.log(this.expenses);
      },
      (error) =>{
        console.log('An error occurred: ', error);
      },
    );
  }

  confirmExpenses() {
    if (this.currentUser.isManager) {
      if (this.expenses && this.expenses.length > 0 && this.expenses[0]?.expense && this.expenses[0]?.expense.isApproved === false) {
        this.customMessageService.showErrorMessage('"The  cash register cannot be closed as long as there are unapproved expenses.');
        return;
      }
      this.router.navigate(['/navbar/close-monthly-activities']);
    }
    else {
      if (this.monthlyRegister.refundAmount === 0) {
        // If refundAmount is 0, show an error and return
        this.customMessageService.showErrorMessage('Refund amount cannot be 0');
        return;
      }
      this.additionalActionsService.closeMonthlyActivities().subscribe(
        (respose) => {
          console.log(respose);
          this.customMessageService.showSuccessMessage("expenses approved successfully")
          this.monthlyCashRegisterService.deactivateMonthlyCashRegister();
          this.router.navigate(['/navbar/home-secretary']);
        },
        (error) => {
          console.error('An error occurred while approve expenses: ', error);
          this.customMessageService.showErrorMessage('An error occurred while approve expenses');
        }
      )
    }
  }

  deleteExpense(event: Event, expense: any) {
    const expenseIdToDelete = expense.expense.expenseId;
    this.confirmationService.confirm({
      target: event.target as EventTarget,
      message: this.translateService.instant('messages.deleteExpenseConfirmation'),
      icon: 'pi pi-info-circle',
      acceptButtonStyleClass: ' p-button-sm',
      accept: () => {
        this.expenseService.deleteExpense(expenseIdToDelete).subscribe(
          (data) => {
            console.log('expense is deleted', data);
            this.customMessageService.showSuccessMessage(this.translateService.instant('messages.expenseDeleted'));
            this.expenses = this.expenses.filter((val) => val.expense.expenseId !== expenseIdToDelete);
          },
          (error) => {
            console.error('An error occurred:', error);
            this.customMessageService.showErrorMessage('An error occurred while deleting the expense');
          }
        );
      },
      reject: () => {
        // this.customMessageService.showRejectedMessage('You have rejected');
      }
    });
  }


  async editExpense(expense: any) {
    console.log(expense);
    this.expense = expense;
    this.expenseDialog = true;
    await this.loadData();
    this.initializeForm();

    const selectedEvent = this.events ? this.events.find(e => e.eventId === expense.expense.eventsId) : null;
    const selectedBuyer = this.buyers ? this.buyers.find(b => b.buyerId == expense.expense.buyerId) : null;
    const selectedExpenseCategory = this.expensesCategory ? this.expensesCategory.find(ec => ec.expenseCategoryId == expense.expense.expenseCategoryId) : null;

    this.formGroup.setValue({
      selectedEvent: selectedEvent || null,
      selectedExpenseCategory: selectedExpenseCategory,
      storeName: expense.expense.storeName,
      expenseAmount: expense.expense.expenseAmount,
      expenseDate: new Date(expense.expense.expenseDate), // Assuming expenseDate is a string
      selectedBuyer: selectedBuyer,
      notes: expense.expense.notes,
    });
    console.log(this.formGroup.value)
  }

  async loadData() {
    await Promise.all([this.loadEvents(), this.loadExpenseCategories(), this.loadBuyers()]);
  }

  hideDialog() {
    this.expenseDialog = false;
    this.submitted = false;
  }

  updateExense() {
    const expenseIdToUpdate = this.expense.expense.expenseId;
    this.validForm = !this.formGroup.invalid;
    if (this.validForm) {
      const refundMonth = this.monthlyCashRegisterService.getCurrentMothlyRegister().monthlyCashRegisterMonth;
      const { selectedBuyer, selectedEvent, selectedExpenseCategory, expenseAmount, expenseDate, storeName, notes } = this.formGroup.value;

      const updatedExpense: NewExpenseModel = {
        expenseId: expenseIdToUpdate,
        buyerId: selectedBuyer?.buyerId,
        eventsId: selectedEvent?.eventId || 2,
        departmentId: this.currentUser.departmentId,
        expenseCategoryId: selectedExpenseCategory?.expenseCategoryId,
        expenseAmount,
        expenseDate: expenseDate?.toISOString(),
        storeName,
        notes: notes,
        isActive: true,
        isApproved: false,
        isLocked: false,
        refundMonth: refundMonth,
        updatedBy: "",
        invoiceScan: ""
      };

      console.log("newExpenseModel", updatedExpense);
      this.expenseService.updateExpense(updatedExpense).subscribe(
        (data) => {
          console.log('expense is updated', data);
          this.customMessageService.showSuccessMessage('Expense is updated');
          if (this.year && this.month)
            this.loadExpensesByYearAndMonth(this.year, this.month);
          this.hideDialog();
        },
        (error) => {
          console.error('An error occurred while updating the expense:', error);
          this.customMessageService.showErrorMessage('An error occurred while updating the expense')
        }
      );
      this.expense = undefined;
    }
  }

  loadEvents(): Promise<void> {
    return new Promise<void>((resolve, reject) => {
      this.eventService.getEventsByUser().subscribe(
        (data) => {
          this.events = data.data;
          resolve();
        },
        (error) => {
          console.error('An error occurred:', error);
          reject(error);
        }
      );
    });
  }

  loadExpenseCategories(): Promise<void> {
    return new Promise<void>((resolve, reject) => {
      this.expenseCategoryService.getAllExpenseCategories().subscribe(
        (data) => {
          this.expensesCategory = data.data;
          resolve();
        },
        (error) => {
          console.error('An error occurred:', error);
          reject(error);
        }
      );
    });
  }

  loadBuyers(): Promise<void> {
    return new Promise<void>((resolve, reject) => {
      this.buyerService.getBuyers().subscribe(
        (data) => {
          this.buyers = data.data;
          resolve();
        },
        (error) => {
          console.error('An error occurred:', error);
          reject(error);
        }
      );
    });
  }

  getOptionLabel(dropdownType: string): string {
    const translationKey = dropdownType === 'expenseCategory' ? 'expenseCategoryName' : 'buyerName';
    return this.translateService.currentLang === 'en-US' ? translationKey : `${translationKey}Heb`;
  }
  getExpenseCategoryName(expenseId: number): string {
    const selectedExpense = this.expenses.find(expense => expense.expense.expenseId === expenseId);
    if (selectedExpense && selectedExpense.expenseCategoryName) {
      return this.translateService.currentLang === 'en-US' ? selectedExpense.expenseCategoryName
        : selectedExpense.expenseCategoryNameHeb;
    }
    return '';
  }


  isInvalid(controlName: string): boolean {
    const control: AbstractControl | null = this.formGroup.get(controlName);
    return control ? control.touched && control.invalid : false;
  }

  initializeForm() {
    this.formGroup = new FormGroup({
      selectedEvent: new FormControl<Event | null>(null),
      selectedExpenseCategory: new FormControl<ExpenseCategory | null>(null, Validators.required,),
      storeName: new FormControl<string | null>(null, Validators.required),
      expenseAmount: new FormControl<number | null>(null, Validators.required),
      expenseDate: new FormControl<Date | null>(null, Validators.required),
      selectedBuyer: new FormControl<any | null>(null),
      notes: new FormControl<string | null>(null),
    });
  }
}