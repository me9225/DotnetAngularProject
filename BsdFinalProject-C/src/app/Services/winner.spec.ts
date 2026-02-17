import { TestBed } from '@angular/core/testing';

import { Winner } from './winner';

describe('Winner', () => {
  let service: Winner;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Winner);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
