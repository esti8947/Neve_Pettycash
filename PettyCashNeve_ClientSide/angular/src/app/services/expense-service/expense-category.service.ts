import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_CONFIG } from 'src/app/config/api.config';

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

  deleteExpenseCategory(expenseCategoryId:number):Observable<any>{
    const url = `${this.baseUrlExpenseCategory}/deleteExpenseCategory/${expenseCategoryId}`;
    return this.http.delete<any>(url);
  }
}
