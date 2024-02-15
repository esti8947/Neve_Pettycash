import { TestBed } from '@angular/core/testing';

import { MonthNameService } from './month-name.service';

describe('MonthNameService', () => {
  let service: MonthNameService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MonthNameService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
