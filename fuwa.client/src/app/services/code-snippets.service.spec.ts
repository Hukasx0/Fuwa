import { TestBed } from '@angular/core/testing';

import { CodeSnippetsService } from './code-snippets.service';

describe('CodeSnippetsService', () => {
  let service: CodeSnippetsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CodeSnippetsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
