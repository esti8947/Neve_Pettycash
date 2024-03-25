import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { AuthService } from 'src/app/services/auth-service/auth.service';
import { ExpenseService } from 'src/app/services/expense-service/expense.service';
import { MonthNameService } from 'src/app/services/month-name/month-name.service';

@Component({
  selector: 'previous-expenses',
  templateUrl: './previous-expenses.component.html',
  styleUrls: ['./previous-expenses.component.scss']
})
export class PreviousExpensesComponent implements OnInit {
  formGroup!: FormGroup;
  currentUser: any;
  expenses: any[] = [];
  cols: any[] = [];
  years: any[] = [];
  monthNames: string[] = [
    'January', 'February', 'March', 'April', 'May', 'June', 'July',
    'August', 'September', 'October', 'November', 'December'
  ];
  monthNamesHeb: string[] = [
    'ינואר', 'פברואר', 'מרץ', 'אפריל', 'מאי', 'יוני', 'יולי',
    'אוגוסט', 'ספטמבר', 'אוקטובר', 'נובמבר', 'דצמבר'
  ];

  constructor(
    private expenseService: ExpenseService,
    private authService: AuthService,
    private monthNameService:MonthNameService,
    private router: Router,
    private translateService: TranslateService,
  ) { }

  ngOnInit(): void {
    const uniqueMonth = this.getUniqueMonths();

    this.currentUser = this.authService.getCurrentUser();
    this.formGroup = new FormGroup({
      selectedYear: new FormControl<number | null>(new Date().getFullYear()),
    });

    this.formGroup.get('selectedYear')?.valueChanges.subscribe((selectedYear: number) => {
      this.getExpensesByYear(selectedYear);
    });

    const initiallySelectedYear = this.formGroup.get('selectedYear')?.value;
    if (initiallySelectedYear) {
      this.getExpensesByYear(initiallySelectedYear.toString());
    }

    this.cols = [
      { field: 'month', header: this.translateService.currentLang === 'en-US' ? 'Month' : 'חודש' },
      { field: 'total', header: this.translateService.currentLang === 'en-US' ? 'Total Expenses' : 'סכום הוצאות' },
    ];

    uniqueMonth.forEach((month) => {
      this.cols.push({
        field: month,
      });
    });

    this.years = this.generateYears(2022, new Date().getFullYear());


  }

  getUniqueMonths(): number[] {
    const validExpenses = this.expenses.filter(expense => expense.expenseDate);
    return Array.from(new Set(validExpenses.map(expense => new Date(expense.expenseDate).getMonth() + 1)));
  }


  getExpenseData(): any[] {
    if (this.expenses.length === 0) {
      return [];
    }
    const uniqueMonths = this.getUniqueMonths();

    // Prepare data for each row
    const dataRows = uniqueMonths.map((month) => {
      const rowData: any = { month: this.getMonthName(month) };
      rowData.total = this.calculateTotalExpensesForMonth(month);
      return rowData;
    });

    // Add the total row for the year
    const totalRow: any = { month: 'Total' };
    totalRow.total = this.calculateTotalExpensesForYear();
    dataRows.push(totalRow);

    return dataRows;
  }

  getMonthName(monthNumber: number | undefined): string {
    return this.monthNameService.getMonthName(monthNumber);
  }
  getMonthNumber(monthName: string): number {

    const monthIndex = this.monthNames.indexOf(monthName);
    if(monthIndex !== -1){
      return monthIndex+1;
    }
    const monthIndexHeb = this.monthNamesHeb.indexOf(monthName);
    if(monthIndexHeb !== -1){
      return monthIndexHeb+1;
    }
    return NaN;
  }


  calculateTotalExpensesForYear(): string {
    const validExpenses = this.expenses.filter(e => e.expenseDate);
    const totalExpenses = validExpenses.reduce((total, expense) => total + expense.expenseAmount, 0);

    // Using toLocaleString to format the number with commas and keeping 2 decimal places
    return totalExpenses.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 });
  }


  calculateTotalExpensesForMonth(month: number): string {
    const validExpenses = this.expenses.filter(expense => expense.expenseDate);
    const totalExpenses = validExpenses
      .filter(expense => new Date(expense.expenseDate).getMonth() + 1 === month)
      .reduce((total, expense) => total + expense.expenseAmount, 0);

    // Using toLocaleString to format the number with commas and keeping 2 decimal places
    return totalExpenses.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 });
  }



  generateYears(startYear: number, endYear: number): number[] {
    const years = [];
    for (let year = startYear; year <= endYear; year++) {
      years.push(year);
    }
    return years;
  }

  getExpensesByYear(year: number) {
    if (this.currentUser.isManager) {
      var departmentId = this.currentUser.departmentId;
    }
    this.expenseService.getExpensesOfUserByYear(year, departmentId).subscribe(
      (data) => {
        this.expenses = data.data || [];
      },
      (error) => {
        console.error('An error occurred:', error);
      }
    )
  };

  showExpensesDetails(selectedYear: number, selectedMonth: number | string) {
    let monthNumber: number;

    if (typeof selectedMonth === 'number') {
      // If selectedMonth is already a number, use it directly
      monthNumber = selectedMonth;
    } else {
      // If selectedMonth is a string, convert it to a number using getMonthNumber
      monthNumber = this.getMonthNumber(selectedMonth);
    }

    if (!isNaN(monthNumber)) {
      if (this.currentUser.isManager) {
        this.router.navigate(['/navbar/expense-report'], {
          queryParams: { selectedYear, selectedMonth: monthNumber }
        });
      } else {
        this.router.navigate(['/navbar/expense-report'], {
          queryParams: { selectedYear, selectedMonth: monthNumber }
        });
      }
    } else {
      console.error('Invalid month name:', selectedMonth);
    }
  }
}

