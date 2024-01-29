import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExpensesReportPreviousMonthsComponent } from './expenses-report-previous-months.component';

describe('ExpensesReportPreviousMonthsComponent', () => {
  let component: ExpensesReportPreviousMonthsComponent;
  let fixture: ComponentFixture<ExpensesReportPreviousMonthsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ExpensesReportPreviousMonthsComponent],
    });
    fixture = TestBed.createComponent(ExpensesReportPreviousMonthsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
