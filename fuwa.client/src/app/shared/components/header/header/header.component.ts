import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {
  isLoggedIn: boolean = false;
  username: string = "you";

  items: any[] = [];

  constructor(private router: Router) {
    this.updateMenu();
  }

  private updateMenu(): void {
    if (this.isLoggedIn) {
      this.items = [
        { label: `Welcome, ${this.username}`, icon: 'pi pi-user', items: [
          { label: 'My Profile', icon: 'pi pi-user', routerLink: `/${this.username}` },
          { label: 'Settings', icon: 'pi pi-cog', routerLink: '/settings' },
          { label: 'Logout', icon: 'pi pi-sign-out' }
        ]},
        { label: 'Home', icon: 'pi pi-home', routerLink: '/' },
        { label: 'Posts', icon: 'pi pi-at', routerLink: '/posts' },
        { label: 'Browse', icon: 'pi pi-hashtag', routerLink: '/browse' }
      ];
    } else {
      this.items = [
        { label: 'Home', icon: 'pi pi-home', routerLink: '/' },
        { label: 'Posts', icon: 'pi pi-at', routerLink: '/posts' },
        { label: 'Browse', icon: 'pi pi-hashtag', routerLink: '/browse' },
        { label: 'Register', icon: 'pi pi-user', routerLink: '/register' },
        { label: 'Login', icon: 'pi pi-sign-in', routerLink: '/login' }
      ];
    }
  }
}