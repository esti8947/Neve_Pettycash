import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { AuthService } from '../auth-service/auth.service';

@Injectable({
  providedIn: 'root'
})
export class BudgetImformationService {
  private baseUrl = 'https://localhost:7139/api/BudgetType';
  constructor(private http:HttpClient, private authService:AuthService) { }


  getBudgetType():Observable<any>{
    const budgetTypeId = this.authService.getCurrentUser().department.currentBudgetTypeId;
    console.log(budgetTypeId);
    const url = `${this.baseUrl}/getBudgetTypeById/${budgetTypeId}`;
    return this.http.get<any>(url).pipe(
      catchError((error)=>{
        console.error('Error in GetEvenetsByUser function', error);
        return throwError(error);
      }),
    );
  }

    getBudgetInformation(departmentId?: number): Observable<any> {
      let url = `${this.baseUrl}/getBudgetInformation`;
      if (departmentId !== undefined && departmentId !== null) {
        url += `?departmentIdFromAdmin=${departmentId}`;
      }
      return this.http.get<any>(url).pipe(
        catchError((error) => {
          console.error('Error in getBudgetInformation function', error);
          return throwError(error);
        }),
      );
    }
    
}
