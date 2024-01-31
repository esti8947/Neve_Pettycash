import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_CONFIG } from 'src/app/config/api.config';

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
}
