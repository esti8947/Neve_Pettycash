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
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

@Component({
  selector: 'app-department-information',
  templateUrl: './department-information.component.html',
  styleUrls: ['./department-information.component.scss']
})
export class DepartmentInformationComponent implements OnInit {
  departmentsArray: DepartmentMoreInfo[] = [];
  tableRowStyle = 'font-size: 14px; padding-left: 8px;';

  constructor(
    private departmentService: DepartmentService,
    private budgetInformationService: BudgetImformationService,
    private monthlyCashRegisterService: MontlyCashRegisterService,
    private expenseService: ExpenseService,
    private authService: AuthService,
    private router: Router,
    private translateService: TranslateService,
    private monthNameService: MonthNameService,
    private sanitizer: DomSanitizer) { }

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
                this.calculateExpensePercentageLastYear(department.monthlyCashRegister, department.departmentId, department.budgetInformation);
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

  calculateExpensePercentageLastYear(monthlyCashRegister: any, departmentId: number, budgetInformation: any) {
    const department = this.departmentsArray.find(dep => dep.departmentId === departmentId);

    if (department != undefined) {

      const month = monthlyCashRegister.monthlyCashRegisterMonth;
      const year = monthlyCashRegister.monthlyCashRegisterYear;

      if (department.currentBudgetTypeId == 1) {
        // Fetch data for current month of the previous year
        this.expenseService.getExpensesAmountForAcademicYearAndMonthOfDepartment(month, year - 1, departmentId).subscribe(
          (data) => {
            department.amountWastedForCalculatingPercentages = data.data;
          },
          (error) => {
            console.error('An error occurred:', error);
          },
        );

        // Fetch data for previous month of the current year
        this.expenseService.getExpensesAmountForAcademicYearAndMonthOfDepartment(month, year, departmentId).subscribe(
          (data) => {
            department.monthlyAmountForCalculatingPercentages = data.data;
          },
          (error) => {
            console.error('An error occurred:', error);
          },
        );
      } else if (department.currentBudgetTypeId == 2) {
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

              department.monthlyAmountForCalculatingPercentages = 8000;
              // department.amountWastedForCalculatingPercentages =
              //   monthlyCashRegister.amountInCashRegister + result4.data;
              department.amountWastedForCalculatingPercentages =
                result4.data + budgetInformation.monthlyBudget.monthlyBudgetCeiling;
            } else {
              // Odd month
              department.monthlyAmountForCalculatingPercentages = result1.data + result3.data;
              // department.amountWastedForCalculatingPercentages =
              //   monthlyCashRegister.amountInCashRegister;
              department.amountWastedForCalculatingPercentages =
                budgetInformation.monthlyBudget.monthlyBudgetCeiling;
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

  }

  getSpentPercentage(totalAmount: number, annualBudgetCeiling: number): number {
    return (totalAmount / annualBudgetCeiling) * 100;
  }

  getDisplayPercentages(departmentId: number): SafeHtml {
    const department = this.departmentsArray.find(dep => dep.departmentId === departmentId);
    if (!department) return '';

    const month = department.monthlyCashRegister.monthlyCashRegisterMonth;
    let year = department.monthlyCashRegister.monthlyCashRegisterYear;
    if (month < 9) year--;

    const lastYear = year - 1;
    const nextYear = year + 1;
    const lastYearRange = `${lastYear}-${year}`;
    const currentYearRange = `${year}-${nextYear}`;

    const tableHeaders = this.getTableHeaders(department, month);

    const tableContent = `
      <tr>
        <th style="${this.tableRowStyle}">${this.translateService.instant('Year')}</th>
        ${tableHeaders}
      </tr>
      <tr>
        <td style="${this.tableRowStyle}">${lastYearRange}</td>
        <td style="${this.tableRowStyle}">₪${department.monthlyAmountForCalculatingPercentages}</td>
      </tr>
      <tr>
        <td style="${this.tableRowStyle}">${currentYearRange}</td>
        <td style="${this.tableRowStyle}">₪${department.amountWastedForCalculatingPercentages}</td>
      </tr>
    `;

    const tableHtml = `<table>${tableContent}</table>`;

    return this.sanitizer.bypassSecurityTrustHtml(tableHtml);
  }

  private getTableHeaders(department: any, month: number): string {
    if (department.currentBudgetTypeId === 1) {
      return `<th  style="${this.tableRowStyle}">${this.translateService.instant('departmentInformation.Expenses until')} ${this.getMonthName(month)}</th>`;
    } else if (department.currentBudgetTypeId === 2 && month % 2 === 0) {
      return `<th  style="${this.tableRowStyle}">${this.translateService.instant('departmentInformation.Expenses months')} ${this.getMonthName(month - 1)} - ${this.getMonthName(month)}</th>`;
    } else {
      return `<th  style="${this.tableRowStyle}">${this.translateService.instant('departmentInformation.Expenses months')} ${this.getMonthName(month)} - ${this.getMonthName(month + 1)}</th>`;
    }
  }


  //       return `Expenses amount for last year ${lastYearRange} unthil month ${monthName} is ₪${department.monthlyAmountForCalculatingPercentages}
  //               Expenses amount for current year ${currentYearRange} unthil current month ${monthName} is ₪${department.amountWastedForCalculatingPercentages}`;
  //     } else
  //     if (department.currentBudgetTypeId == 2) {
  //       if(month % 2 === 0){
  //         return `Expenses amoutn for last year  ${lastYearRange} in monthes ${this.getMonthName(month - 1)} - ${monthName} is ₪${department.monthlyAmountForCalculatingPercentages}
  //         Expenses amoutn for current year ${currentYearRange} in monthes ${this.getMonthName(month - 1)} - ${monthName} expnese amount for month ${this.getMonthName(month - 1)} and expense plan for month ${monthName} is ₪${department.amountWastedForCalculatingPercentages} `;
  //       }else{
  //         return `Expenses amoutn for last year  ${lastYearRange} in monthes ${monthName} - ${this.getMonthName(month + 1)} is ₪${department.monthlyAmountForCalculatingPercentages}
  //         Expenses amoutn for current year ${currentYearRange} in monthes ${monthName} expnese plan for month ${monthName} is ₪${department.amountWastedForCalculatingPercentages}`;

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