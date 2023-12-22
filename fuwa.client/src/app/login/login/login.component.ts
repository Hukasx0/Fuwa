import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
 loginForm: FormGroup;

 constructor(private formbuilder: FormBuilder) {
  this.loginForm = this.formbuilder.group({
    loginOrEmail: ['', Validators.required],
    password: ['', Validators.required]
  });
 }

 isFormInvalid(): boolean {
  return this.loginForm.invalid || this.loginForm.pristine;
}

 onSubmit() {
  console.log(this.loginForm.value);
 }
}
