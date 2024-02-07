import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LockExpensesButtonComponent } from './lock-expenses-button.component';

describe('LockExpensesButtonComponent', () => {
  let component: LockExpensesButtonComponent;
  let fixture: ComponentFixture<LockExpensesButtonComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LockExpensesButtonComponent]
    });
    fixture = TestBed.createComponent(LockExpensesButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
