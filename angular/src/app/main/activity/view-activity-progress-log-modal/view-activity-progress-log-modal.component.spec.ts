/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { ViewActivityProgressLogModalComponent } from './view-activity-progress-log-modal.component';

describe('ViewActivityProgressLogModalComponent', () => {
  let component: ViewActivityProgressLogModalComponent;
  let fixture: ComponentFixture<ViewActivityProgressLogModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewActivityProgressLogModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewActivityProgressLogModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
