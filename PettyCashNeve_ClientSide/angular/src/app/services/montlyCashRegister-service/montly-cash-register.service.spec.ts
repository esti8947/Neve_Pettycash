import { TestBed } from '@angular/core/testing';

import { MontlyCashRegisterService } from './montly-cash-register.service';

describe('MontlyCashRegisterService', () => {
  let service: MontlyCashRegisterService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MontlyCashRegisterService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
