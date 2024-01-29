import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { NewExpenseModel } from '../../models/expense';
import { catchError } from 'rxjs/operators';
@Injectable({
  providedIn: 'root'
})
export class ExpenseReportInfoService {
  private baseUrl = "https://localhost:7139/api/ExpenseMoreInfo"

  constructor(private http:HttpClient) { }

   getExpenseReportOfUser():Observable<any>{
    const url =`${this.baseUrl}/getExpensesReportOfCurrentMonth`;
    return this.http.get<any>(url).pipe(
      catchError((error) =>{
        console.error('Error in GetExpensesReportOfUser function', error)
        return throwError(error)
      }),
    );
   }

   getExpensesReportByYearAndMonth(year:number, month:number, departmentId?:number):Observable<any>{
    let url = `${this.baseUrl}/getExpensesByYearAndMonth/${year}/${month}`;
    if (departmentId !== undefined && departmentId !== null) {
      url += `?departmentId=${departmentId}`;
    }
    return this.http.get<any>(url).pipe(
      catchError((error) =>{
        console.error('Error in getExpensesReportByYearAndMonth function', error)
        return throwError(error)
      }),
    );
  }
}
