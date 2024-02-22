import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { API_CONFIG } from 'src/app/config/api.config';
import { Department } from 'src/app/models/department';

@Injectable({
  providedIn: 'root',
})
export class DepartmentService {
  private baseUrl = API_CONFIG.baseUrl;

  private baseUrlDepartment = `${this.baseUrl}/Department`;

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
    const url = `${this.baseUrlDepartment}/getDepartments`;
    return this.http.get<any>(url).pipe(
      catchError((error) =>{
        console.error('Error in getAllDepartments function', error);
        return throwError(error);
      }),
    );
  }

  getDepartmentById(id: number): Observable<any> {
    const url = `${this.baseUrlDepartment}/GetDepartmentById/${id}`;
    return this.http.get(url);
  }
  
  addDepartment(newDepartment:Department):Observable<any>{
    const url = `${this.baseUrlDepartment}/createDepartment`;
    return this.http.post<Department>(url, newDepartment).pipe(
      catchError((error)=>{
        console.error('Error in addDepartment function', error);
        return throwError(error)
      }),
    );
  }
}
