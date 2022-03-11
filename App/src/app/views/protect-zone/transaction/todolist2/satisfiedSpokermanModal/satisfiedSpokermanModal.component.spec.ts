/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { SatisfiedSpokermanModalComponent } from './satisfiedSpokermanModal.component';

describe('SatisfiedSpokermanModalComponent', () => {
  let component: SatisfiedSpokermanModalComponent;
  let fixture: ComponentFixture<SatisfiedSpokermanModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SatisfiedSpokermanModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SatisfiedSpokermanModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
