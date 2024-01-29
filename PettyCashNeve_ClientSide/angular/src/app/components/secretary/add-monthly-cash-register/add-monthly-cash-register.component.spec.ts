import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddMonthlyCashRegisterComponent } from './add-monthly-cash-register.component';

describe('AddMonthlyCashRegisterComponent', () => {
  let component: AddMonthlyCashRegisterComponent;
  let fixture: ComponentFixture<AddMonthlyCashRegisterComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AddMonthlyCashRegisterComponent]
    });
    fixture = TestBed.createComponent(AddMonthlyCashRegisterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
