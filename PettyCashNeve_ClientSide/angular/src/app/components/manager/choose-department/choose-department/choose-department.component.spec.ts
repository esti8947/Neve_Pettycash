import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseDepartmentComponent } from './choose-department.component';

describe('ChooseDepartmentComponent', () => {
  let component: ChooseDepartmentComponent;
  let fixture: ComponentFixture<ChooseDepartmentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ChooseDepartmentComponent]
    });
    fixture = TestBed.createComponent(ChooseDepartmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
