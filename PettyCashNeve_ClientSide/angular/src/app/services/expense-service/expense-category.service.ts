import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { API_CONFIG } from 'src/app/config/api.config';
import { ExpenseCategory } from 'src/app/models/expenseCategory';

@Injectable({
  providedIn: 'root',
})
export class ExpenseCategoryService {
  private baseUrl = API_CONFIG.baseUrl;

  private baseUrlExpenseCategory = `${this.baseUrl}/ExpenseCategory`;

  constructor(private http: HttpClient) {}

  getAllExpenseCategories(): Observable<any> {
    const url = `${this.baseUrlExpenseCategory}/getAllExpensesCategory`;
    return this.http.get<any>(url);
  }
  getActiveAndInactiveExpenseCategoryAsync(): Observable<any> {
    const url = `${this.baseUrlExpenseCategory}/getActiveAndInactiveExpenseCategoryAsync`;
    return this.http.get<any>(url);
  }

  deleteExpenseCategory(expenseCategoryId:number):Observable<any>{
    const url = `${this.baseUrlExpenseCategory}/deleteExpenseCategory/${expenseCategoryId}`;
    return this.http.delete<any>(url);
  }

  updateExpenseCategory(expenseCategory:ExpenseCategory):Observable<any>{
    const url = `${this.baseUrlExpenseCategory}/updateExpenseCategory`;
    return this.http.put<ExpenseCategory>(url, expenseCategory).pipe(
      catchError((error)=>{
        console.error('Error in updateDepartment function', error);
        return throwError(error)
      }),
    );
  }

  addExpenseCategory(newExpenseCategory:ExpenseCategory):Observable<any>{
    const url = `${this.baseUrlExpenseCategory}/createExpenseCategory`;
    return this.http.post<ExpenseCategory>(url, newExpenseCategory).pipe(
      catchError((error)=>{
        console.error('Error in addDepartment function', error);
        return throwError(error)
      }),
    );
  }
  activateExpenseCategory(expenseCategoryId: number): Observable<any> {
    const url = `${this.baseUrlExpenseCategory}/activateExpenseCategory/${expenseCategoryId}`;
    return this.http.get(url).pipe(
      catchError((error) => {
        console.error('Error in activateExpenseCategory function', error);
        return throwError(error);
      }),
    );
  }
}
