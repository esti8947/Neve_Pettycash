import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { DepartmentMoreInfo } from 'src/app/models/departmentMoreInfo';

@Injectable({
  providedIn: 'root'
})
export class DepartmentDataService {
  private departmentsArraySubject = new BehaviorSubject<DepartmentMoreInfo[]>([]);
  departmentsArray$ = this.departmentsArraySubject.asObservable();

  constructor() {}

  updateDepartmentsArray(departments: DepartmentMoreInfo[]) {
    this.departmentsArraySubject.next(departments);
  }
}
