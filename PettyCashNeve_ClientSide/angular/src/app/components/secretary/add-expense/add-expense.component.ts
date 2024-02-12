import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ExpenseCategory } from 'src/app/models/expenseCategory';
import { EventService } from 'src/app/services/event-service/event.service';
import { Event } from 'src/app/models/event';
import { ExpenseCategoryService } from 'src/app/services/expense-service/expense-category.service';
import { BuyerService } from 'src/app/services/buyer-service/buyer.service';
import { Router, ActivatedRoute } from '@angular/router';
import { AbstractControl } from '@angular/forms';
import { NewExpenseModel } from 'src/app/models/expense';
import { ExpenseService } from 'src/app/services/expense-service/expense.service';
import { CustomMessageService } from 'src/app/services/customMessage-service/custom-message.service';
import { MontlyCashRegisterService } from 'src/app/services/montlyCashRegister-service/montly-cash-register.service';
import { AuthService } from 'src/app/services/auth-service/auth.service';
import { TranslateService } from '@ngx-translate/core';
import { formatDate } from '@angular/common';

@Component({
  selector: 'add-expense',
  templateUrl: './add-expense.component.html',
  styleUrls: ['./add-expense.component.scss'],
})
export class AddExpenseComponent implements OnInit {
  currentUser:any;
  expensesCategory: ExpenseCategory[] = [];
  events: Event[] = [];
  buyers: any[] = [];
  formGroup!: FormGroup;
  validForm: boolean = true;
  isEvents: boolean = false;
  selectedEventParams:Event | undefined;
  isMonthlyCashRegister:boolean = false;
  currentMonth:number | undefined;
  minDateValue: Date = new Date();
  maxDateValue: Date = new Date();
  

  constructor(
    private readonly translateService:TranslateService,
    private authService:AuthService,
    private expenseCategoryService: ExpenseCategoryService,
    private buyerService: BuyerService,
    private eventService: EventService,
    private expenseService:ExpenseService,
    private router: Router,
    private activedRoute: ActivatedRoute,
    private customMessageService:CustomMessageService,
    private monthlyCashRegisterService:MontlyCashRegisterService,
  ) {}

  ngOnInit() {
    this.activedRoute.queryParams.subscribe(params =>{
      const selectedEventString = params['selectedEvent'];
      this.selectedEventParams = selectedEventString ? JSON.parse(selectedEventString) : undefined;
      console.log(this.selectedEventParams);
    })
    this.loadUser();
    this.loadEvents();
    this.loadExpenseCategories();
    this.loadBuyers();
    this.initializeForm();
    this.isMonthlyRegister();
  }
  loadUser(){
    this.currentUser = this.authService.getCurrentUser();
    console.log(this.currentUser);
  }

  isMonthlyRegister(){
    const isMonthlyRegisterSet = this.monthlyCashRegisterService.getCurrentMothlyRegister();
    if(isMonthlyRegisterSet)
    this.isMonthlyCashRegister = true;
    this.currentMonth = isMonthlyRegisterSet.monthlyCashRegisterMonth;
    this.setMinMaxDates();
  }
  
  setMinMaxDates() {
    if (this.currentMonth) {
      const year = new Date().getFullYear();
      const month = this.currentMonth;
      const firstDayOfMonth = new Date(Number(year), Number(month) - 1, 1);
      const lastDayOfMonth = new Date(Number(year), Number(month), 0);
      this.minDateValue = firstDayOfMonth;
      this.maxDateValue = lastDayOfMonth;
    }
  }

  loadEvents() {
    this.eventService.getEventsByUser().subscribe(
      (data) => {
        this.events = data.data;
        if (this.events?.length > 0 || this.events != null) {
          this.isEvents = true;
        }
      },
      (error) => {
        console.error('An error occurred:', error);
      },
    );
  }

  loadExpenseCategories() {
    this.expenseCategoryService.getAllExpenseCategories().subscribe(
      (data) => {
        this.expensesCategory = data.data;
        console.log("expensesCategory",this.expensesCategory)
      },
      (error) => {
        console.error('An error occurred:', error);
      },
    );
  }

  loadBuyers() {
    this.buyerService.getBuyers().subscribe(
      (data) => {
        this.buyers = data.data;
        console.log(this.buyers)
      },
      (error) => {
        console.error('An error occurred:', error);
      },
    );
  }

  isInvalid(controlName: string): boolean {
    const control: AbstractControl | null = this.formGroup.get(controlName);
    return control ? control.touched && control.invalid : false;
  }

  getOptionLabel(dropdownType: string): string {
    const translationKey = dropdownType === 'expenseCategory' ? 'expenseCategoryName' : 'buyerName';
    return this.translateService.currentLang === 'en-US' ? translationKey : `${translationKey}Heb`;
  }
  
  
  addExpense() {
    this.validForm = !this.formGroup.invalid;
    if(this.validForm && this.isMonthlyCashRegister){
      const refundMonth  =  this.monthlyCashRegisterService.getCurrentMothlyRegister().monthlyCashRegisterMonth;
      const { selectedBuyer, selectedEvent, selectedExpenseCategory, expenseAmount, expenseDate, storeName, notes } = this.formGroup.value;
      const formattedExpenseDate = formatDate(expenseDate, 'yyyy-MM-ddTHH:mm:ss', 'en-US');

      const newExpenseModel: NewExpenseModel = {
        expenseId: 0,
        buyerId: selectedBuyer?.buyerId,
        eventsId: selectedEvent?.eventId || 2,
        departmentId: this.currentUser.departmentId,
        expenseCategoryId: selectedExpenseCategory?.expenseCategoryId,
        expenseAmount,
        // expenseDate: expenseDate?.toISOString(),
        expenseDate:formattedExpenseDate,
        storeName,
        notes: notes || "",
        isActive: true,
        isApproved: false,
        isLocked: false,
        refundMonth: refundMonth,
        updatedBy: "",
        invoiceScan:""
      };
  
      console.log("newExpenseModel", newExpenseModel);

      this.expenseService.addNewExpense(newExpenseModel).subscribe(
        (response) =>{
          console.log("expense added successfully", response);
          this.customMessageService.showSuccessMessage('Expense is added');
          this.formGroup.reset();
        },
        (error)=>{
          console.error('An error occurred while add the expense:', error);
          this.customMessageService.showErrorMessage('An error occurred while adding the expense');
        }
      );
    }
    else{
      this.customMessageService.showErrorMessage('Cannot add expenses before setting up a monthly cash register.')
    }
  }
  cancelExpense() {
    this.formGroup.reset();
    this.router.navigate(['/home-department']);
  }
  initializeForm() {
    this.formGroup = new FormGroup({
      selectedEvent: new FormControl<Event | null>(this.selectedEventParams ?? null, null),
      selectedExpenseCategory: new FormControl<ExpenseCategory | null>(null,Validators.required,),
      storeName: new FormControl<string | null>(null, Validators.required),
      expenseAmount: new FormControl<number | null>(null, Validators.required),
      expenseDate: new FormControl<Date | null>(null, Validators.required),
      selectedBuyer: new FormControl<any | null>(null),
      notes: new FormControl<string | null>(null),
    });
  }
}
