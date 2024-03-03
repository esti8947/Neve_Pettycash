import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { forkJoin } from 'rxjs';
import { Department } from 'src/app/models/department';
import { DepartmentMoreInfo } from 'src/app/models/departmentMoreInfo';
import { MonthlyCashRegister } from 'src/app/models/monthlyCashRegister';
import { AuthService } from 'src/app/services/auth-service/auth.service';
import { BudgetImformationService } from 'src/app/services/budget-information-service/budget-imformation.service';
import { DepartmentService } from 'src/app/services/department-service/department.service';
import { ExpenseService } from 'src/app/services/expense-service/expense.service';
import { MonthNameService } from 'src/app/services/month-name/month-name.service';
import { MontlyCashRegisterService } from 'src/app/services/montlyCashRegister-service/montly-cash-register.service';

@Component({
  selector: 'app-department-information',
  templateUrl: './department-information.component.html',
  styleUrls: ['./department-information.component.scss']
})
export class DepartmentInformationComponent implements OnInit {
  departmentsArray: DepartmentMoreInfo[] = [];


  constructor(
    private departmentService: DepartmentService,
    private budgetInformationService: BudgetImformationService,
    private monthlyCashRegisterService: MontlyCashRegisterService,
    private expenseService: ExpenseService,
    private authService: AuthService,
    private router: Router,
    private translateService: TranslateService,
    private monthNameService: MonthNameService) { }

  ngOnInit(): void {
    this.loadDepartments();
  }

  loadDepartments() {
    this.departmentService.getAllDepartments().subscribe(
      (data) => {
        this.departmentsArray = data.data;
        this.getBudgetAndCashRegisterDetails();
      },
      (error) => {
        console.error('An error occurred:', error);
      },
    );
  }

  selectDepartment(selectedDepartment: any) {
    if (selectedDepartment) {
      this.departmentService.saveSelectedDepartmentToLocalStorage(selectedDepartment);

      this.authService.updateCurrentUserDepartmentId(selectedDepartment.departmentId);
      this.router.navigate(['/navbar']);
    }
  }

  getBudgetAndCashRegisterDetails() {
    this.departmentsArray.forEach(department => {
      this.budgetInformationService.getBudgetInformation(department.departmentId).subscribe(
        (data) => {
          department.budgetInformation = data.data;
          if (department.budgetInformation.annualBudget != null) {
            department.budgetType = "annualBudget";
          } else {
            if (department.budgetInformation.monthlyBudget != null) {
              department.budgetType = "monthlyBudget";
            } else {
              department.budgetType = "refundBudget"
            }
          }
        },
        (error) => {
          console.error('An error occurred while fetching budget information:', error);
        }
      );

      this.monthlyCashRegisterService.getCurrentMontlyCashRegisterByUserId(department.departmentId).subscribe(
        (data) => {
          department.monthlyCashRegister = data.data[0];

          if (department.monthlyCashRegister != undefined) {
            this.expenseService.getExpensesAmountOfDepartmentByYearandMonth(department.monthlyCashRegister.monthlyCashRegisterMonth, department.monthlyCashRegister.monthlyCashRegisterYear, department.departmentId).subscribe(
              (data) => {
                department.totalExpensesAmount = data.data;
                this.calculateExpensePercentageLastYear(department.monthlyCashRegister, department.departmentId);
              },
              (error) => {
                console.error('An error occurred in getExpensesAmountOfDepartmentByYearandMonth function', error);
              }
            )
          }
        },
        (error) => {
          console.error('An error occurred while fetching monthly cash register details:', error);
        }
      );
    });
    console.log("departmentsArray", this.departmentsArray)
  }

  calculateExpensePercentageLastYear(monthlyCashRegister: any, departmentId: number) {
    const department = this.departmentsArray.find(dep => dep.departmentId === departmentId);

    if(department != undefined){

      const month = monthlyCashRegister.monthlyCashRegisterMonth;
      const year = monthlyCashRegister.monthlyCashRegisterYear;
  
      forkJoin([
        this.expenseService.getExpensesAmountOfDepartmentByYearandMonth(month, year - 1, departmentId),
        this.expenseService.getExpensesAmountOfDepartmentByYearandMonth(month - 1, year - 1, departmentId),
        this.expenseService.getExpensesAmountOfDepartmentByYearandMonth(month + 1, year - 1, departmentId),
        this.expenseService.getExpensesAmountOfDepartmentByYearandMonth(month - 1, year, departmentId),
      ]).subscribe(
        ([result1, result2, result3, result4]) => {
          console.log(result1, result2, result3, result4);
          if (month % 2 === 0) {
            // Even month
            department.monthlyAmountForCalculatingPercentages = result1.data + result2.data;
            department.amountWastedForCalculatingPercentages =
              monthlyCashRegister.amountInCashRegister + result4.data;
          } else {
            // Odd month
            department.monthlyAmountForCalculatingPercentages = result1.data + result3.data;
            department.amountWastedForCalculatingPercentages =
              monthlyCashRegister.amountInCashRegister;
          }
          console.log(
            department.amountWastedForCalculatingPercentages,
            department.monthlyAmountForCalculatingPercentages
          );
        },
        (error) => {
          console.error('Error getting expenses data', error);
        }
      );
    }
    
  }

  getSpentPercentage(totalAmount: number, annualBudgetCeiling: number): number {
    return (totalAmount / annualBudgetCeiling) * 100;
  }

  getBudgetTypeName(departmentId: number): string {
    const department = this.departmentsArray.find(dep => dep.departmentId === departmentId);
    if (department) {
      return this.translateService.currentLang === 'en-US' ?
        department.budgetInformation?.budgetType?.budgetTypeName :
        department.budgetInformation?.budgetType?.budgetTypeNameHeb;
    }
    return '';
  }

  getMonthName(monthNumber: number | undefined): string {
    return this.monthNameService.getMonthName(monthNumber);
  }


}
