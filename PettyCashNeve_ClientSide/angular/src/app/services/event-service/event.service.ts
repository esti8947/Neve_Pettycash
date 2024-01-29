import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { Event } from 'src/app/models/event';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class EventService {
  private baseUrl = 'https://localhost:7139/api/Event';

  constructor(private http: HttpClient) {}

  addNewEvent(newEvent:any):Observable<boolean>{
    const url = `${this.baseUrl}/createEvent`;
    return this.http.post<any>(url, newEvent).pipe(
      catchError((error) => {
        console.error('Error in addNewEvents function:', error);
        return throwError(error);
      })
    );
  }

  getEventsByUser():Observable<any>{
    const url = `${this.baseUrl}/getEventsByUserAndMonth`;
    return this.http.get<any>(url).pipe(
      catchError((error)=>{
        console.error('Error in GetEvenetsByUser function', error);
        return throwError(error);
      }),
    );
  }
}
