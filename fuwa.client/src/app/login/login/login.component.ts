import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Login } from 'src/app/models/login';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
 loginForm: FormGroup;

 ngOnInit() {
  if (localStorage.getItem('auth')) {
    this.router.navigate(['/']);
  }
 }

 constructor(private formbuilder: FormBuilder,
             private authService: AuthenticationService,
             private router: Router,
             private snackBar: MatSnackBar
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
      localStorage.setItem('auth', response.token);
      this.router.navigate(['/']);
    },
    error: (error) => {
      console.error('Login error:', error);
      this.showSnackbarError(error.error || 'Login error occurred');
    }
  });
 }
 private showSnackbarError(message: string): void {
  this.snackBar.open(message, 'Close', {
    duration: 5000
   });
 }
}
