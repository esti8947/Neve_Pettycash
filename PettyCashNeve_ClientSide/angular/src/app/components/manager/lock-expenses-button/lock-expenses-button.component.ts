import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { AuthService } from 'src/app/services/auth-service/auth.service';
import { DepartmentService } from 'src/app/services/department-service/department.service';
import { ExpenseService } from 'src/app/services/expense-service/expense.service';
import { MonthNameService } from 'src/app/services/month-name/month-name.service';

@Component({
  selector: 'lock-expenses-button',
  templateUrl: './lock-expenses-button.component.html',
  styleUrls: ['./lock-expenses-button.component.scss']
})
export class LockExpensesButtonComponent implements OnInit {

  currnetUser: any;
  unlockedExpensesList: any[] = [];
  uniqueMonthsAndYears: string[] = [];

  constructor(private expenseService: ExpenseService, private authService: AuthService, private departmentService: DepartmentService,
    private router: Router, private translateService: TranslateService,
    private monthNameService:MonthNameService) { }

  ngOnInit(): void {
    this.currnetUser = this.authService.getCurrentUser();
    this.currnetUser &&
      this.loadUnlockedExpenses(this.currnetUser.departmentId);
  }

  loadUnlockedExpenses(departmentId: number) {
    this.expenseService.GetUnlockedExpensesOfDepartment(departmentId).subscribe(
      (data) => {
        this.unlockedExpensesList = data.data || [];
        console.log("unlockedExpensesList", this.unlockedExpensesList)
        this.extractUniqueMonthsAndYears();
      },
      (error) => {
        console.log('An error occurred: ', error);
      }
    )
  };

  extractUniqueMonthsAndYears() {
    this.unlockedExpensesList.forEach(expense => {
      const expenseDate = new Date(expense.expenseDate);
      const monthYear = expenseDate.toLocaleString('default', { month: 'long' }) + ' ' + expenseDate.getFullYear();

      if (!this.uniqueMonthsAndYears.includes(monthYear)) {
        this.uniqueMonthsAndYears.push(monthYear);
      }
    });
  }

  getButtonText(monthYear: string): string {
    const [month, year] = monthYear.split(' ');
    const monthNumber = new Date(Date.parse(month + ' 1, ' + year)).getMonth() + 1;

    const monthName = this.monthNameService.getMonthName(monthNumber);

    return this.translateService.currentLang === 'en-US' ?
      `Expense Lock for ${monthName} ${year}` :
      `נעילת הוצאות לחודש ${monthName} ${year}`;
  }

  navigateToExpenseReport(monthYear: string) {
    const [month, year] = monthYear.split(' ');
    const monthNumber = new Date(Date.parse(month + ' 1, ' + year)).getMonth() + 1;

    this.router.navigate(['/navbar/expense-report'], { queryParams: { selectedMonth: monthNumber, selectedYear: year } });
  }
}
