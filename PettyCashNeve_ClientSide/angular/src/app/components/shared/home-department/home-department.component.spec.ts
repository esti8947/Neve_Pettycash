import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeDepartmentComponent } from './home-department.component';

describe('HomeDepartmentComponent', () => {
  let component: HomeDepartmentComponent;
  let fixture: ComponentFixture<HomeDepartmentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HomeDepartmentComponent],
    });
    fixture = TestBed.createComponent(HomeDepartmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
