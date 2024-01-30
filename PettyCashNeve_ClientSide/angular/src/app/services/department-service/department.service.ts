import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DepartmentService {
  private baseUrl = 'https://localhost:7139/api/Department';

  constructor(private http: HttpClient) {}

  saveSelectedDepartmentToLocalStorage(response:any){
    const responseString = JSON.stringify(response);
    localStorage.setItem('selected_department', responseString)
  }
  deactivateSelectedDepartment() {
    localStorage.removeItem('selected_department');
  }

  getSelectedDepartment(): any | null {
    const storedResponseString = localStorage.getItem('selected_department');

    if (storedResponseString) {
      return JSON.parse(storedResponseString);
    }
    return null;
  }

  getAllDepartments():Observable<any>{
    const url = `${this.baseUrl}/getDepartments`;
    return this.http.get<any>(url).pipe(
      catchError((error) =>{
        console.error('Error in getAllDepartments function', error);
        return throwError(error);
      }),
    );
  }

  getDepartmentById(id: number): Observable<any> {
    const url = `${this.baseUrl}/GetDepartmentById/${id}`;
    return this.http.get(url);
  }
}
