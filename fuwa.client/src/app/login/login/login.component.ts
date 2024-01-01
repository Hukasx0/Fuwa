import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Login } from 'src/app/models/login';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
 loginForm: FormGroup;

 ngOnInit() {
  if (this.authService.isLoggedIn()) {
    this.router.navigate(['/']);
  }
 }

 constructor(private formbuilder: FormBuilder,
             private authService: AuthenticationService,
             private router: Router
             ) {
  this.loginForm = this.formbuilder.group({
    userTagOrMail: ['', Validators.required],
    password: ['', Validators.required]
  });
 }

 isFormInvalid(): boolean {
  return this.loginForm.invalid || this.loginForm.pristine;
}

onSubmit() {
  if (this.isFormInvalid()) {
    return;
  }
  const loginData: Login = {
    userTagOrMail: this.loginForm.get('userTagOrMail')?.value as string,
    password: this.loginForm.get('password')?.value as string,
  };
  
  this.authService.login(loginData).subscribe({
    next: (response) => {
      this.router.navigate(['/']);
    },
    error: (error) => {
      console.error(error);
    }
  })

  }
 /*private showSnackbarError(message: string): void {
  this.snackBar.open(message, 'Close', {
    duration: 5000
   });
 }*/
}
