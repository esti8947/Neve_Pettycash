import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BudgetInformationComponent } from './budget-information.component';

describe('BudgetInformationComponent', () => {
  let component: BudgetInformationComponent;
  let fixture: ComponentFixture<BudgetInformationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [BudgetInformationComponent],
    });
    fixture = TestBed.createComponent(BudgetInformationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
