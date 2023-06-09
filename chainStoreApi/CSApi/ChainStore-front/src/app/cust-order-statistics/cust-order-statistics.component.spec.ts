import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustOrderStatisticsComponent } from './cust-order-statistics.component';

describe('CustOrderStatisticsComponent', () => {
  let component: CustOrderStatisticsComponent;
  let fixture: ComponentFixture<CustOrderStatisticsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CustOrderStatisticsComponent]
    });
    fixture = TestBed.createComponent(CustOrderStatisticsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
