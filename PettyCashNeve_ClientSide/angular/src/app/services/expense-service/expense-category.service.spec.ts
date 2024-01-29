import { TestBed } from '@angular/core/testing';

import { ExpenseCategoryServiceService } from './expense-category.service';

describe('ExpenseCategoryServiceService', () => {
  let service: ExpenseCategoryServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ExpenseCategoryServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
