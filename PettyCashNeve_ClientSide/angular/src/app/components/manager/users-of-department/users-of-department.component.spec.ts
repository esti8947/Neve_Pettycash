import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UsersOfDepartmentComponent } from './users-of-department.component';

describe('UsersOfDepartmentComponent', () => {
  let component: UsersOfDepartmentComponent;
  let fixture: ComponentFixture<UsersOfDepartmentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UsersOfDepartmentComponent]
    });
    fixture = TestBed.createComponent(UsersOfDepartmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
