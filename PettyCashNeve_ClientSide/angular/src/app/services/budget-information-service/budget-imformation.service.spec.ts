import { TestBed } from '@angular/core/testing';

import { BudgetImformationService } from './budget-imformation.service';

describe('BudgetImformationService', () => {
  let service: BudgetImformationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BudgetImformationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
