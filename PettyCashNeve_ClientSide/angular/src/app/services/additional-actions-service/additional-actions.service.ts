import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdditionalActionsService {
  private baseUrl = 'https://localhost:7139/api/Orchestration';

  constructor(private http:HttpClient) { }

  closeMonthlyActivities():Observable<any>{
    const url = `${this.baseUrl}/closeMonthlyActivities`;
    return this.http.get<any>(url).pipe(
      catchError((error) =>{
        console.error('Error in CloseMonthlyActivities function', error);
        return(error);
      })
    )
  }
}
