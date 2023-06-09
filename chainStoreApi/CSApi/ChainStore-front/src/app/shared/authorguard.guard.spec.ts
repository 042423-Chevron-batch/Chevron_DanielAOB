import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { authorguardGuard } from './authorguard.guard';

describe('authorguardGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => authorguardGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
