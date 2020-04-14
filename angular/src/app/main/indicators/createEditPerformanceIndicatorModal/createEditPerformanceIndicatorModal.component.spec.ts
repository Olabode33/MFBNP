/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { CreateEditPerformanceIndicatorModalComponent } from './createEditPerformanceIndicatorModal.component';

describe('CreateEditPerformanceIndicatorModalComponent', () => {
  let component: CreateEditPerformanceIndicatorModalComponent;
  let fixture: ComponentFixture<CreateEditPerformanceIndicatorModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateEditPerformanceIndicatorModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateEditPerformanceIndicatorModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
