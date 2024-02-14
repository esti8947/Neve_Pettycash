import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { Event } from 'src/app/models/event';
import { catchError } from 'rxjs/operators';
import { API_CONFIG } from 'src/app/config/api.config';

@Injectable({
  providedIn: 'root',
})
export class EventService {
  private baseUrl = API_CONFIG.baseUrl;

  private baseUrlEvent = `${this.baseUrl}/Event`;

  constructor(private http: HttpClient) {}

  addNewEvent(newEvent:any):Observable<boolean>{
    const url = `${this.baseUrlEvent}/createEvent`;
    return this.http.post<any>(url, newEvent).pipe(
      catchError((error) => {
        console.error('Error in addNewEvents function:', error);
        return throwError(error);
      })
    );
  }

  getEventsByUser():Observable<any>{
    const url = `${this.baseUrlEvent}/getEventsByUserAndMonth`;
    return this.http.get<any>(url).pipe(
      catchError((error)=>{
        console.error('Error in GetEvenetsByUser function', error);
        return throwError(error);
      }),
    );
  }

  deleteEvent(eventId:number):Observable<any>{
    const url = `${this.baseUrlEvent}/deleteEvent/${eventId}`;
    return this.http.delete<any>(url).pipe(
      catchError((error)=>{
        console.error('Error in deleteEvent function', error);
        return throwError(error);
      }),
    );
  }

  updateEvent(updatedEvent:any):Observable<any>{
    const url = `${this.baseUrlEvent}/UpdateEvent`;
    return this.http.put<any>(url, updatedEvent).pipe(
      catchError((error)=>{
        console.error('Error in updateEvent function', error);
        return throwError(error);
      }),
    );

  }
}
