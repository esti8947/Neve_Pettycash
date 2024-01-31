import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { API_CONFIG } from 'src/app/config/api.config';

@Injectable({
  providedIn: 'root'
})
export class BudgetTypeService {
  private baseUrl = API_CONFIG.baseUrl;

  private baseUrlBudgetType = `${this.baseUrl}/BudgetType`;

  constructor(private http:HttpClient) { }

  getBudgetTypes():Observable<any>{
    const url = `${this.baseUrlBudgetType}/getBudgetTypeList`;
    return this.http.get<any>(url).pipe(
      catchError((error)=>{
        console.error('Error in GetBudgetTypes function',error);
        return throwError(error)
      }),
    )
  }
}