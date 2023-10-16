import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CarsGridComponent } from './cars-grid.component';

describe('CarsGridComponent', () => {
  let component: CarsGridComponent;
  let fixture: ComponentFixture<CarsGridComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CarsGridComponent]
    });
    fixture = TestBed.createComponent(CarsGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
