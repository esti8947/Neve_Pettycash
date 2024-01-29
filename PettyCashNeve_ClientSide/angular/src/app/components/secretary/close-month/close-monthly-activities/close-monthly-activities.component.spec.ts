import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CloseMonthlyActivitiesComponent } from './close-monthly-activities.component';

describe('CloseMonthlyActivitiesComponent', () => {
  let component: CloseMonthlyActivitiesComponent;
  let fixture: ComponentFixture<CloseMonthlyActivitiesComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CloseMonthlyActivitiesComponent]
    });
    fixture = TestBed.createComponent(CloseMonthlyActivitiesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
