import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ExpenseCategoryService {
  private baseUrl = 'https://localhost:7139/api/ExpenseCategory';

  constructor(private http: HttpClient) {}

  getAllExpenseCategories(): Observable<any> {
    const url = `${this.baseUrl}/getAllExpensesCategory`;
    return this.http.get<any>(url);
  }

  deleteExpenseCategory(expenseCategoryId:number):Observable<any>{
    const url = `${this.baseUrl}/deleteExpenseCategory/${expenseCategoryId}`;
    return this.http.delete<any>(url);
  }
}
