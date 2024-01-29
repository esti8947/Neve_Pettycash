import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { EventCategory } from 'src/app/models/eventCategory';
@Injectable({
  providedIn: 'root',
})
export class EventCategoryService {
  private baseUrl = 'https://localhost:7139/api/EventCategory';

  constructor(private http: HttpClient) {}

  getEventsCategories(): Observable<any> {
    return this.http.get<EventCategory>(
      `${this.baseUrl}/getEventCategoriesAsync`,
    );
  }
}
