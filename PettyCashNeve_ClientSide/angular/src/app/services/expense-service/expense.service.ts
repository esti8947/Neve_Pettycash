import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { NewExpenseModel } from '../../models/expense';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class ExpenseService {
  private baseUrl = 'https://localhost:7139/api/Expense';

  constructor(private http: HttpClient) {}

  // getExpensesOfUser(): Observable<any> {
  //   const url = `${this.baseUrl}/GetExpensesOfUser`;
  //   return this.http.get<any>(url).pipe(
  //     catchError((error) =>{
  //       console.error('Error in GetExpensesOfUser function', error)
  //       return throwError(error)
  //     }),
  //   );
  // }

  getExpensesOfUserByYear(year:number): Observable<any> {
    const url = `${this.baseUrl}/GetExpensesOfUserByYear/${year}`;
    return this.http.get<any>(url).pipe(
      catchError((error) =>{
        console.error('Error in GetExpensesOfUserByYear function', error)
        return throwError(error)
      }),
    );
  }

  getExpensesAmountOfUserByYearandMonth(month:number, year:number): Observable<any> {
    const url = `${this.baseUrl}/GetExpensesAmountForMonth/${month}/${year}`;
    return this.http.get<any>(url).pipe(
      catchError((error) =>{
        console.error('Error in GetExpensesOfUserByYear function', error)
        return throwError(error)
      }),
    );
  }

  GetUnapprovedExpensesByUserAsync(): Observable<any> {
    const url = `${this.baseUrl}/GetUnapprovedExpensesByUserAsync`;
    return this.http.get<any>(url).pipe(
      catchError((error) =>{
        console.error('Error in GetExpensesOfUser function', error)
        return throwError(error)
      }),
    );
  }


  getAllExpenses(): Observable<any> {
    const url = `${this.baseUrl}/getAllExpenses`
    return this.http.get<any>(url);
  }

  updateExpense(updatedExpense: NewExpenseModel): Observable<any> {
    const url = `${this.baseUrl}/UpdateExpense`;
    return this.http.put<NewExpenseModel>(url, updatedExpense).pipe(
      catchError((error)=>{
        console.error('Error in updateExpense function', error);
        return throwError(error)
      }),
    );
  }

  deleteExpense(id: number): Observable<any> {
    const url = `${this.baseUrl}/DeleteExpense/${id}`;
    return this.http.delete(url);
  }

  addNewExpense(newExpense:NewExpenseModel):Observable<any>{
    const url = `${this.baseUrl}/createExpense`;
    return this.http.post<NewExpenseModel>(url, newExpense).pipe(
      catchError((error)=>{
        console.error('Error in addNewExpense function', error);
        return throwError(error)
      }),
    );
  }
}
