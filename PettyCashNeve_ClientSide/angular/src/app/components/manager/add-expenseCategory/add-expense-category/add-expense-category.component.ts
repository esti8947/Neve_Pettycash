import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ConfirmationService } from 'primeng/api';
import { CustomMessageService } from 'src/app/services/customMessage-service/custom-message.service';
import { ExpenseCategoryService } from 'src/app/services/expense-service/expense-category.service';

@Component({
  selector: 'add-expense-category',
  templateUrl: './add-expense-category.component.html',
  styleUrls: ['./add-expense-category.component.scss']
})
export class AddExpenseCategoryComponent implements OnInit {
  expenseCategoryDialog:boolean = false;
  selectedExpenseCategory:any;
  existingExpensesCategory:any[] = [];
  formGroup!:FormGroup;
  validForm:boolean = true;

  constructor(
    private formBuilder:FormBuilder,
    private expenseCategoryService:ExpenseCategoryService,
    private confirmationService:ConfirmationService,
    private customMessageService:CustomMessageService
  ){}

  ngOnInit(): void {
    this.loadExpensesCategory();
    this.initializeForm();
  }

  addExpenseCategory(){
    this.expenseCategoryDialog = true;
  }

  loadExpensesCategory(){
    this.expenseCategoryService.getAllExpenseCategories().subscribe(
      (data) => {
        this.existingExpensesCategory = data.data;
        console.log(this.existingExpensesCategory);
      },
      (error) => {
        this.existingExpensesCategory = [
          {id:1, name:"food"},
          {id:2, name:"taxi"},
          {id:3, name:"lactures"}
        ]
        console.error('An error occurred:', error);        
      },
    );
  }

  isInvalid(controlName: string): boolean {
    const control: AbstractControl | null = this.formGroup.get(controlName);
    return control ? control.touched && control.invalid : false;
  }

  initializeForm(){

    this.formGroup = this.formBuilder.group({
      expenseCategoryName: new FormControl<string|null>(null, Validators.required),
      expenseCategoryCode:new FormControl<string|null>(null,Validators.required)
    });
  }

  deleteExpenseCategory(event: Event, expenseCategory:any) {
    const expenseCategoryIdToDelete = expenseCategory.id;
    console.log(expenseCategoryIdToDelete)
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
              this.existingExpensesCategory = this.existingExpensesCategory.filter((val) => val.expenseCategory.id !== expenseCategoryIdToDelete);
            },
            (error) => {
              console.error('An error occurred:', error);
              this.customMessageService.showErrorMessage('An error occurred while deleting the expense category');
            }
          );        
        },
        reject: () => {
          this.customMessageService.showRejectedMessage('You have rejected');
        }
    });
  }

  saveExpenseCategory(){
    
  }
}
