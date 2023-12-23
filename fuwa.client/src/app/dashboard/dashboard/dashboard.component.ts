import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent {
  ngOnInit() {
    if (!localStorage.getItem('auth')) {
      this.router.navigate(['/login']);
    }
   }

   constructor(private router: Router) { }
}
