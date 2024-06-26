import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject, catchError, tap, throwError } from 'rxjs';
import { API_CONFIG } from 'src/app/config/api.config';
import { Department } from 'src/app/models/department';

@Injectable({
  providedIn: 'root',
})
export class DepartmentService {
  private departmentAddedSubject = new Subject<void>();

  departmentAdded$ = this.departmentAddedSubject.asObservable();
  
  private baseUrl = API_CONFIG.baseUrl;

  private baseUrlDepartment = `${this.baseUrl}/Department`;

  constructor(private http: HttpClient) { }

  saveSelectedDepartmentToLocalStorage(response: any) {
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

  getAllDepartments(): Observable<any> {
    const url = `${this.baseUrlDepartment}/getDepartments`;
    return this.http.get<any>(url).pipe(
      catchError((error) => {
        console.error('Error in getAllDepartments function', error);
        return throwError(error);
      }),
    );
  }

  getInactiveDepartments(): Observable<any> {
    const url = `${this.baseUrlDepartment}/getInactiveDepartments`;
    return this.http.get<any>(url).pipe(
      catchError((error) => {
        console.error('Error in getInactiveDepartments function', error);
        return throwError(error);
      }),
    );
  }

  getDepartmentById(id: number): Observable<any> {
    const url = `${this.baseUrlDepartment}/GetDepartmentById/${id}`;
    return this.http.get(url).pipe(
      tap((response: any) => {
        if (response && response.data) {
          this.saveSelectedDepartmentToLocalStorage(response.data); // Save response.data to local storage
        }
      }),
      catchError((error) => {
        console.error('Error in getDepartmentById function', error);
        return throwError(error);
      })
    );
  }


  addDepartment(newDepartment: Department): Observable<any> {
    const url = `${this.baseUrlDepartment}/createDepartment`;
    return this.http.post<Department>(url, newDepartment).pipe(
      catchError((error) => {
        console.error('Error in addDepartment function', error);
        return throwError(error)
      }),
      tap(() => {
        this.departmentAddedSubject.next();
      })
    );
  }

  deleteDepartment(departmentId: number): Observable<any> {
    const url = `${this.baseUrlDepartment}/deleteDepartment/${departmentId}`;
    return this.http.delete(url).pipe(
      catchError((error) => {
        console.error('Error in deleteDepartment function', error);
        return throwError(error);
      }),
    );
  }
  activateDepartment(departmentId: number): Observable<any> {
    const url = `${this.baseUrlDepartment}/activateDepartment/${departmentId}`;
    return this.http.get(url).pipe(
      catchError((error) => {
        console.error('Error in activateDepartment function', error);
        return throwError(error);
      }),
    );
  }
  updateDepartment(updatedDepartment: Department): Observable<any> {
    const url = `${this.baseUrlDepartment}/updateDepartment`;
    return this.http.put<Department>(url, updatedDepartment).pipe(
      catchError((error)=>{
        console.error('Error in updateDepartment function', error);
        return throwError(error)
      }),
    );
  }
}
