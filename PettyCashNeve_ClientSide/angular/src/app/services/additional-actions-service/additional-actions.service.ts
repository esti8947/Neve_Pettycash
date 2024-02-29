import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError } from 'rxjs';
import { API_CONFIG } from 'src/app/config/api.config';
import { NewYear } from 'src/app/models/newYear';

@Injectable({
  providedIn: 'root'
})
export class AdditionalActionsService {
  private baseUrl = API_CONFIG.baseUrl;
  private baseUrlOrchestration = `${this.baseUrl}/Orchestration`;

  constructor(private http: HttpClient) { }

  closeMonthlyActivities(year: number, month: number): Observable<any> {
    const url = `${this.baseUrlOrchestration}/closeMonthlyActivities/${year}/${month}`;
    return this.http.get<any>(url).pipe(
      catchError((error) => {
        console.error('Error in CloseMonthlyActivities function', error);
        return (error);
      })
    )
  }

  addAmountToBudget(departmentId: number, amount: number): Observable<any> {
    const url = `${this.baseUrlOrchestration}/addAmountToBudget/${departmentId}/${amount}`;
    return this.http.get<any>(url).pipe(
      catchError((error) => {
        console.error('Error in addAmountToBudget function', error);
        return (error);
      })
    )
  }

  getUsersOfDepartment(departmentId: number): Observable<any> {
    const url = `${this.baseUrlOrchestration}/getUsersOfDepartment/${departmentId}`;
    return this.http.get<any>(url).pipe(
      catchError((error) => {
        console.error('Error in getUsersOfDepartment function', error);
        return (error);
      })
    )
  }

  openNewYear(newYear:NewYear): Observable<any> {
    let url = `${this.baseUrlOrchestration}/closeLastYearAndOpenNewYearActivities`;
    return this.http.post<any>(url, newYear).pipe(
      catchError((error) => {
        console.error('Error in openNewYear function', error);
        return (error);
      })
    )
  }
}
