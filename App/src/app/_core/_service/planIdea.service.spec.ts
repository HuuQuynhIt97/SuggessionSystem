/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { PlanIdeaService } from './planIdea.service';

describe('Service: PlanIdea', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [PlanIdeaService]
    });
  });

  it('should ...', inject([PlanIdeaService], (service: PlanIdeaService) => {
    expect(service).toBeTruthy();
  }));
});
