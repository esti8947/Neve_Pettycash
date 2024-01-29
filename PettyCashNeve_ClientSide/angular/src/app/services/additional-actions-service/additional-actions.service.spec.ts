import { TestBed } from '@angular/core/testing';

import { AdditionalActionsService } from './additional-actions.service';

describe('AdditionalActionsService', () => {
  let service: AdditionalActionsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AdditionalActionsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
