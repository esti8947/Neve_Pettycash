import { TestBed } from '@angular/core/testing';

import { BudgetTypeService } from './budget-type.service';

describe('BudgetTypeService', () => {
  let service: BudgetTypeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BudgetTypeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
