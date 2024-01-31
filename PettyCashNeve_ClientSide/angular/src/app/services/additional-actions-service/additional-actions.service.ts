import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError } from 'rxjs';
import { API_CONFIG } from 'src/app/config/api.config';

@Injectable({
  providedIn: 'root'
})
export class AdditionalActionsService {
  private baseUrl = API_CONFIG.baseUrl;
  private baseUrlOrchestration = `${this.baseUrl}/Orchestration`;

  constructor(private http:HttpClient) { }

  closeMonthlyActivities():Observable<any>{
    const url = `${this.baseUrlOrchestration}/closeMonthlyActivities`;
    return this.http.get<any>(url).pipe(
      catchError((error) =>{
        console.error('Error in CloseMonthlyActivities function', error);
        return(error);
      })
    )
  }
}
