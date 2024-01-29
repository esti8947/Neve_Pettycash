import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MonthlyCashRegisterComponent } from './monthly-cash-register.component';

describe('MonthlyCashRegisterComponent', () => {
  let component: MonthlyCashRegisterComponent;
  let fixture: ComponentFixture<MonthlyCashRegisterComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MonthlyCashRegisterComponent],
    });
    fixture = TestBed.createComponent(MonthlyCashRegisterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
