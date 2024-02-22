import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { API_CONFIG } from 'src/app/config/api.config';
import { Buyer } from 'src/app/models/buyer';

@Injectable({
  providedIn: 'root',
})
export class BuyerService {
  private baseUrl = API_CONFIG.baseUrl;

  private baseUrlBuyer = `${this.baseUrl}/Buyer`;
  constructor(private http: HttpClient) {}

  getBuyers(): Observable<any> {
    const url = `${this.baseUrlBuyer}/getBuyers`;
    return this.http.get<any>(url);
  }

  addBuyer(newBuyer:Buyer):Observable<any>{
    const url = `${this.baseUrlBuyer}/createBuyer`;
    return this.http.post<Buyer>(url, newBuyer).pipe(
      catchError((error)=>{
        console.error('Error in addDepartment function', error);
        return throwError(error)
      }),
    );
  }

  deleteBuyer(buyerId:number):Observable<any>{
    const url = `${this.baseUrlBuyer}/deleteBuyer/${buyerId}`;
    return this.http.delete<any>(url);
  }
}
