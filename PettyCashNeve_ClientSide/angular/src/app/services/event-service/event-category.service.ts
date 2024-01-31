import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { EventCategory } from 'src/app/models/eventCategory';
import { API_CONFIG } from 'src/app/config/api.config';
@Injectable({
  providedIn: 'root',
})
export class EventCategoryService {
  private baseUrl = API_CONFIG.baseUrl;

  private baseUrlEvent = `${this.baseUrl}/EventCategory`;

  constructor(private http: HttpClient) {}

  getEventsCategories(): Observable<any> {
    return this.http.get<EventCategory>(
      `${this.baseUrlEvent}/getEventCategoriesAsync`,
    );
  }
}
