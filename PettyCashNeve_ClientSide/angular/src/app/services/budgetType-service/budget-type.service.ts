import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BudgetTypeService {
  private baseUrl = 'ttps://localhost:7139/api/BudgetType';

  constructor(private http:HttpClient) { }

  getBudgetTypes():Observable<any>{
    const url = `${this.baseUrl}/getBudgetTypeList`;
    return this.http.get<any>(url).pipe(
      catchError((error)=>{
        console.error('Error in GetBudgetTypes function',error);
        return throwError(error)
      }),
    )
  }
}