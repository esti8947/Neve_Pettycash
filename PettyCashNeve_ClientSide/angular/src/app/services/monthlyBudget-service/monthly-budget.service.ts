import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { API_CONFIG } from 'src/app/config/api.config';
import { MonthlyBudget } from 'src/app/models/monthlyBudget';

@Injectable({
  providedIn: 'root'
})
export class MonthlyBudgetService {
  private baseUrl = API_CONFIG.baseUrl;
  private baseUrlMonthlyBudget = `${this.baseUrl}/MonthlyBudget`

  constructor(private http:HttpClient) { }

  createMonthlyBudget(monthlyBudget:MonthlyBudget):Observable<any>{
    const url = `${this.baseUrlMonthlyBudget}/createMonthlyBudget`;
    return this.http.post<MonthlyBudget>(url, monthlyBudget).pipe(
      catchError((error) => {
        console.error('Error in createMonthlyBudget function:', error);
        return throwError(error);
      })
    );
  }
}
