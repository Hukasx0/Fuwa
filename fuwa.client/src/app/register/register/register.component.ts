import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  registerForm: FormGroup;

  constructor(private formbuilder: FormBuilder) {
    this.registerForm = this.formbuilder.group({
      userTag: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      username: ['', Validators.required],
      password: ['', Validators.required],
      acceptTerms: [false, Validators.requiredTrue]
    });
  }

  isFormInvalid(): boolean {
    return this.registerForm.invalid || 
           this.registerForm.pristine || 
           !this.registerForm.value.acceptTerms;
  }

  onSubmit() {
    console.log(this.registerForm.value);
  }
}
