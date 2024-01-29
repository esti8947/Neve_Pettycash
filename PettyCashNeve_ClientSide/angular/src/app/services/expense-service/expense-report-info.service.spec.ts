import { TestBed } from '@angular/core/testing';

import { ExpenseReportInfoService } from './expense-report-info.service';

describe('ExpenseReportInfoService', () => {
  let service: ExpenseReportInfoService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ExpenseReportInfoService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
