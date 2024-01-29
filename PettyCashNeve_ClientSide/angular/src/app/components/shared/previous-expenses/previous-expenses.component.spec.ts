import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PreviousExpensesComponent } from './previous-expenses.component';

describe('PreviousExpensesComponent', () => {
  let component: PreviousExpensesComponent;
  let fixture: ComponentFixture<PreviousExpensesComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PreviousExpensesComponent]
    });
    fixture = TestBed.createComponent(PreviousExpensesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
