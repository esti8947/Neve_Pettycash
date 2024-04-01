import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { EventCategory } from 'src/app/models/eventCategory';
import { API_CONFIG } from 'src/app/config/api.config';
@Injectable({
  providedIn: 'root',
})
export class EventCategoryService {
  private baseUrl = API_CONFIG.baseUrl;

  private baseUrlEventCategory = `${this.baseUrl}/EventCategory`;

  constructor(private http: HttpClient) {}

  getEventsCategories(): Observable<any> {
    return this.http.get<EventCategory>(
      `${this.baseUrlEventCategory}/getEventCategoriesAsync`,
    );
  }
  getAllEventsCategories(): Observable<any> {
    return this.http.get<EventCategory>(
      `${this.baseUrlEventCategory}/getAllEventCategories`,
    );
  }
  addEventCategory(newEventCategory:EventCategory):Observable<any>{
    const url = `${this.baseUrlEventCategory}/createEventCategory`;
    return this.http.post<EventCategory>(url, newEventCategory).pipe(
      catchError((error)=>{
        console.error('Error in addDepartment function', error);
        return throwError(error)
      }),
    );
  }

  deleteEventCategory(eventCategoryId:number):Observable<any>{
    const url = `${this.baseUrlEventCategory}/deleteEventCategory/${eventCategoryId}`;
    return this.http.delete<any>(url);
  }

  activateEventCategory(eventCategoryId: number): Observable<any> {
    const url = `${this.baseUrlEventCategory}/activateEventCategory/${eventCategoryId}`;
    return this.http.get(url).pipe(
      catchError((error) => {
        console.error('Error in activateEventCategory function', error);
        return throwError(error);
      }),
    );
  }
}
