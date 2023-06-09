import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetproductsStoreComponent } from './getproducts-store.component';

describe('GetproductsStoreComponent', () => {
  let component: GetproductsStoreComponent;
  let fixture: ComponentFixture<GetproductsStoreComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GetproductsStoreComponent]
    });
    fixture = TestBed.createComponent(GetproductsStoreComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
