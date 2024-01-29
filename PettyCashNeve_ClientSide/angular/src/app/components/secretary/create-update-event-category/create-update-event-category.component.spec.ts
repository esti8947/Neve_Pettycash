import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdateEventCategoryComponent } from './create-update-event-category.component';

describe('CreateUpdateEventCategoryComponent', () => {
  let component: CreateUpdateEventCategoryComponent;
  let fixture: ComponentFixture<CreateUpdateEventCategoryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CreateUpdateEventCategoryComponent],
    });
    fixture = TestBed.createComponent(CreateUpdateEventCategoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
