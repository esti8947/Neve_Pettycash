import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { AuthService } from '../auth-service/auth.service';
import { catchError, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class MontlyCashRegisterService {
  private baseUrl =
    'https://localhost:7139/api/MonthlyCashRegister';
  private headers = new HttpHeaders().set('Content-Type', 'application/json');

  constructor(
    private http: HttpClient,
  ) { }

  getCurrentMontlyCashRegisterByUserId(departmentId?: number): Observable<any> {
    let url = `${this.baseUrl}/getMonthlyCashRegistersByUserId`;
    if (departmentId !== undefined && departmentId !== null) {
      url += `?departmentId=${departmentId}`;
    }
    return this.http.get<any>(url).pipe(
      tap((response) => {
        if (Array.isArray(response)) {
          // If response is an array, apply saveResponseToLocalStorage to each item
          response.forEach((item: any) => this.saveResponseToLocalStorage(item));
        } else {
          // If response is a single object, apply saveResponseToLocalStorage to it
          this.saveResponseToLocalStorage(response);
        }
      }),
      catchError((error) => {
        console.error('Error in getMontlyCashRegister function:', error);
        return throwError(error);
      }),
    );
  }

  saveResponseToLocalStorage(response: any) {
    const responseString = JSON.stringify(response.data);
    localStorage.setItem('current_monthlyCashRegister', responseString)
  }
  deactivateMonthlyCashRegister() {
    localStorage.removeItem('current_monthlyCashRegister');
  }

  getCurrentMothlyRegister(): any | null {
    const storedResponseString = localStorage.getItem('current_monthlyCashRegister');

    if (storedResponseString) {
      return JSON.parse(storedResponseString);
    }
    return null;
  }

  addMonthlyRegister(newMonthlyRegister: any): Observable<boolean> {
    const url = `${this.baseUrl}/createNewMonthlyCashRegister`;
    return this.http.post<any>(url, newMonthlyRegister).pipe(
      catchError((error) => {
        console.error('Error in addNewEvents function:', error);
        return throwError(error);
      })
    );
  }

  insertRefundAmount(refundAmount: number): Observable<boolean> {
    const url = `${this.baseUrl}/insertRefundAmount/${refundAmount}`;
    return this.http.get<any>(url).pipe(
      catchError((error) => {
        console.error('Error in insertRefundAmount function:', error);
        return throwError(error);
      })
    );
  }
}
