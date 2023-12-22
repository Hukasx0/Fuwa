import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewCodeSnippetComponent } from './new-code-snippet.component';

describe('NewCodeSnippetComponent', () => {
  let component: NewCodeSnippetComponent;
  let fixture: ComponentFixture<NewCodeSnippetComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [NewCodeSnippetComponent]
    });
    fixture = TestBed.createComponent(NewCodeSnippetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
