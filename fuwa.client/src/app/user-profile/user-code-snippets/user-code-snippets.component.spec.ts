import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserCodeSnippetsComponent } from './user-code-snippets.component';

describe('UserCodeSnippetsComponent', () => {
  let component: UserCodeSnippetsComponent;
  let fixture: ComponentFixture<UserCodeSnippetsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserCodeSnippetsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(UserCodeSnippetsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
