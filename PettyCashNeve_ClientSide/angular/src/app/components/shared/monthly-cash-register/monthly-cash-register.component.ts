import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import {  forkJoin } from 'rxjs';
import { MonthlyCashRegister } from 'src/app/models/monthlyCashRegister';
import { AuthService } from 'src/app/services/auth-service/auth.service';
import { BudgetImformationService } from 'src/app/services/budget-information-service/budget-imformation.service';
import { ExpenseService } from 'src/app/services/expense-service/expense.service';
import { MonthNameService } from 'src/app/services/month-name/month-name.service';

@Component({
  selector: 'monthly-cash-register',
  templateUrl: './monthly-cash-register.component.html',
  styleUrls: ['./monthly-cash-register.component.scss'],
})
export class MonthlyCashRegisterComponent implements OnInit {
  @Input() monthlyRegister:any | undefined

  MonthlyRegisterValue: MonthlyCashRegister = new MonthlyCashRegister();
  budgetInformation: any;
  budgetType: string = "";
  currentUser:any;
  monthlyAmountForCalculatingPercentages: number | undefined;
  amountWastedForCalculatingPercentages: number | undefined;

  constructor(
    private budgetInformationService: BudgetImformationService,
    private expenseService: ExpenseService,
    private authService:AuthService,
    private monthNameService:MonthNameService,
    private router: Router,
  ) { }

  ngOnInit(): void {
    this.currentUser = this.authService.getCurrentUser();
    this.MonthlyRegisterValue = this.monthlyRegister;
    this.getMonthlyRegister(this.MonthlyRegisterValue);
    this.getBudgetInformation();
  }

  getMonthlyRegister(monthlyRegister: MonthlyCashRegister | undefined) {
    if (monthlyRegister)
      this.MonthlyRegisterValue = monthlyRegister;

    if (this.MonthlyRegisterValue.monthlyCashRegisterMonth !== undefined
      && this.MonthlyRegisterValue.monthlyCashRegisterYear !== undefined) {
      this.MonthlyRegisterValue.monthString = this.getMonthName(
        this.MonthlyRegisterValue.monthlyCashRegisterMonth
      );

      // Define month and year only after getMonthlyRegister is successful
      const month = this.MonthlyRegisterValue.monthlyCashRegisterMonth;
      const year = this.MonthlyRegisterValue.monthlyCashRegisterYear;

      forkJoin([
        this.expenseService.getExpensesAmountOfUserByYearandMonth(month, year - 1),
        this.expenseService.getExpensesAmountOfUserByYearandMonth(month - 1, year - 1),
        this.expenseService.getExpensesAmountOfUserByYearandMonth(month + 1, year - 1),
        this.expenseService.getExpensesAmountOfUserByYearandMonth(month - 1, year),
      ]).subscribe(
        ([result1, result2, result3, result4]) => {
          // console.log(result1, result2, result3, result4);
          if (month % 2 === 0) {
            // Even month
            this.monthlyAmountForCalculatingPercentages = result1.data + result2.data;
            this.amountWastedForCalculatingPercentages =
              this.MonthlyRegisterValue.amountInCashRegister + result4.data;
          } else {
            // Odd month
            this.monthlyAmountForCalculatingPercentages = result1.data + result3.data;
            this.amountWastedForCalculatingPercentages =
              this.MonthlyRegisterValue.amountInCashRegister;
          }
          console.log(
            this.amountWastedForCalculatingPercentages,
            this.monthlyAmountForCalculatingPercentages
          );
        },
        (error) => {
          console.error('Error getting expenses data', error);
        }
      );
    }
    (error: any) => {
      console.error('Error getting monthly register data', error);
    }
  }

  getSpentPercentage(totalAmount: number, annualBudgetCeiling: number): number {
    return (totalAmount / annualBudgetCeiling) * 100;
  }

  getMonthName(monthNumber: number | undefined): string {
    return this.monthNameService.getMonthName(monthNumber);
  }

  getBudgetInformation() {
    if(this.currentUser.isManager){
      var departmentId = this.currentUser.departmentId;
    }
    this.budgetInformationService.getBudgetInformation(departmentId).subscribe(
      (data) => {
        this.budgetInformation = data.data;
        if (this.budgetInformation.annualBudget != null) {
          this.budgetType = "annualBudget";
        }
        else {
          if (this.budgetInformation.monthlyBudget != null) {
            this.budgetType = "monthlyBudget";
          }
          else {
            this.budgetType = "refundBudget"
          }
        }
      },
      (error) => {
        console.error('An error occurred:', error);
      },
    );
  }

  navigateToExpenseReport() {
    // Navigate to the desired route when the button is clicked
    this.currentUser.isManager? this.router.navigate(['/navbar/expense-report']):
    this.router.navigate(['/navbar/expense-report']);
  }
  navigateToAddExpense() {
    this.router.navigate(['/navbar/add-expense']);
  }

}