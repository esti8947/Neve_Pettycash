import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DepartmentInformationComponent } from './department-information.component';

describe('DepartmentInformationComponent', () => {
  let component: DepartmentInformationComponent;
  let fixture: ComponentFixture<DepartmentInformationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DepartmentInformationComponent]
    });
    fixture = TestBed.createComponent(DepartmentInformationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
