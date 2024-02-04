import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { NewExpenseModel } from '../../models/expense';
import { catchError } from 'rxjs/operators';
import { API_CONFIG } from 'src/app/config/api.config';
@Injectable({
  providedIn: 'root'
})
export class ExpenseReportInfoService {
  private baseUrl = API_CONFIG.baseUrl;

  private baseUrlExpenseMoreInfo = `${this.baseUrl}/ExpenseMoreInfo`

  constructor(private http: HttpClient) { }

  getExpenseReportOfUser(): Observable<any> {
    const url = `${this.baseUrlExpenseMoreInfo}/getExpensesReportOfCurrentMonth`;
    return this.http.get<any>(url).pipe(
      catchError((error) => {
        console.error('Error in GetExpensesReportOfUser function', error)
        return throwError(error)
      }),
    );
  }

  getExpensesReportByYearAndMonth(year: number, month: number, departmentId?: number): Observable<any> {
    let url = `${this.baseUrlExpenseMoreInfo}/getExpensesByYearAndMonth/${year}/${month}`;
    if (departmentId !== undefined && departmentId !== null) {
      url += `?departmentId=${departmentId}`;
    }
    return this.http.get<any>(url).pipe(
      catchError((error) => {
        console.error('Error in getExpensesReportByYearAndMonth function', error)
        return throwError(error)
      }),
    );
  }

  GetUnLockedExpensesByDepartmentId(departmentId:number):Observable<any>{
    const url = `${this.baseUrlExpenseMoreInfo}/getUnlockedExpenses/${departmentId}`;
    return this.http.get<any>(url).pipe(
      catchError((error) => {
        console.error('Error in getExpensesReportByYearAndMonth function', error)
        return throwError(error)
      }),
    );
  }
}
