import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class BuyerService {
  private baseUrl = 'https://localhost:7139/api/Buyer';
  constructor(private http: HttpClient) {}

  getBuyers(): Observable<any> {
    const url = `${this.baseUrl}/getBuyers`;
    return this.http.get<any>(url);
  }
}
