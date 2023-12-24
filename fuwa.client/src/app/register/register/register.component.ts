import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Register } from 'src/app/models/register';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  registerForm: FormGroup;

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
    if (this.isFormInvalid()) {
      return;
    }
    const registerData: Register = {
      userTag: this.registerForm.get('userTag')?.value as string,
      email: this.registerForm.get('email')?.value as string,
      username: this.registerForm.get('username')?.value as string,
      password: this.registerForm.get('password')?.value as string
    };

    this.authService.register(registerData).subscribe({
      next: (response) => {
        this.router.navigate(['/login']);
      },
      error: (error) => {
        console.error('Register error: ', error);
        this.showSnackbarError(error.error || 'Register error occurred');
      }
    })
  }
  private showSnackbarError(message: string): void {
    this.snackBar.open(message, 'Close', {
      duration: 5000
     });
  }
}
