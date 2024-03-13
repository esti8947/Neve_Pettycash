import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { NewExpenseModel } from '../../models/expense';
import { catchError } from 'rxjs/operators';
import { API_CONFIG } from 'src/app/config/api.config';

@Injectable({
  providedIn: 'root',
})
export class ExpenseService {
  private baseUrl = API_CONFIG.baseUrl;

  private baseUrlExpens = `${this.baseUrl}/Expense`;

  constructor(private http: HttpClient) {}

  getExpensesOfUserByYear(year:number, departmentId?: number): Observable<any> {
    let url = `${this.baseUrlExpens}/GetExpensesOfUserByYear/${year}`;
    if(departmentId !== undefined && departmentId!== null){
      url += `?departmentId=${departmentId}`
    }
    return this.http.get<any>(url).pipe(
      catchError((error) =>{
        console.error('Error in GetExpensesOfUserByYear function', error)
        return throwError(error)
      }),
    );
  }

  getExpensesAmountOfUserByYearandMonth(month:number, year:number): Observable<any> {
    const url = `${this.baseUrlExpens}/GetExpensesAmountForMonth/${month}/${year}`;
    return this.http.get<any>(url).pipe(
      catchError((error) =>{
        console.error('Error in GetExpensesOfUserByYear function', error)
        return throwError(error)
      }),
    );
  }

  getExpensesAmountOfDepartmentByYearandMonth(month:number, year:number, departmentId:number): Observable<any> {
    const url = `${this.baseUrlExpens}/GetExpensesAmountForMonthByDepartmentId/${month}/${year}/${departmentId}`;
    return this.http.get<any>(url).pipe(
      catchError((error) =>{
        console.error('Error in getExpensesAmountOfDepartmentByYearandMonth function', error)
        return throwError(error)
      }),
    );
  }

  getExpensesAmountForAcademicYearAndMonthOfDepartment(month:number, year:number, departmentId:number): Observable<any> {
    const url = `${this.baseUrlExpens}/getExpensesAmountForAcademicYearAndMonthOfDepartment/${month}/${year}/${departmentId}`;
    return this.http.get<any>(url).pipe(
      catchError((error) =>{
        console.error('Error in getExpensesAmountForAcademicYearAndMonthOfDepartment function', error)
        return throwError(error)
      }),
    );
  }

  GetUnapprovedExpensesByUserAsync(): Observable<any> {
    const url = `${this.baseUrlExpens}/GetUnapprovedExpensesByUserAsync`;
    return this.http.get<any>(url).pipe(
      catchError((error) =>{
        console.error('Error in GetUnapprovedExpensesByUserAsync function', error)
        return throwError(error)
      }),
    );
  }

  GetUnlockedExpensesOfDepartment(departmentId:number):Observable<any>{
    const url  = `${this.baseUrlExpens}/getApprovedAndUnlockedExpensesOfDepartment/${departmentId}`;
    return this.http.get<any>(url).pipe(
      catchError((error) =>{
        console.error('Error in GetUnlockedExpensesOfDepartment function', error)
        return throwError(error)
      }),
    );
  }


  getAllExpenses(): Observable<any> {
    const url = `${this.baseUrlExpens}/getAllExpenses`
    return this.http.get<any>(url);
  }

  updateExpense(updatedExpense: NewExpenseModel): Observable<any> {
    const url = `${this.baseUrlExpens}/UpdateExpense`;
    return this.http.put<NewExpenseModel>(url, updatedExpense).pipe(
      catchError((error)=>{
        console.error('Error in updateExpense function', error);
        return throwError(error)
      }),
    );
  }

  deleteExpense(id: number): Observable<any> {
    const url = `${this.baseUrlExpens}/DeleteExpense/${id}`;
    return this.http.delete(url);
  }

  addNewExpense(newExpense:NewExpenseModel):Observable<any>{
    const url = `${this.baseUrlExpens}/createExpense`;
    return this.http.post<NewExpenseModel>(url, newExpense).pipe(
      catchError((error)=>{
        console.error('Error in addNewExpense function', error);
        return throwError(error)
      }),
    );
  }

  lockExpenses(month:number, year:number, departmentId:number):Observable<any>{
    const url  = `${this.baseUrlExpens}/lockExpenses/${month}/${year}/${departmentId}`;
    return this.http.get<any>(url).pipe(
      catchError((error) =>{
        console.error('Error in GetExpensesOfUser function', error)
        return throwError(error)
      }),
    );
  }
}
