import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  ngOnInit() {
    if (localStorage.getItem('auth')) {
      this.router.navigate(['/dashboard']);
    }
   }

   constructor(private router: Router) {}
}
