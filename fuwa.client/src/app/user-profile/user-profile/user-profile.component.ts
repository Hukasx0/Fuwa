import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { combineLatest } from 'rxjs';
import { User } from 'src/app/models/user';
import { UsersService } from 'src/app/services/users.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent {
  user: User | undefined;
  tab: MenuItem | undefined;

  tabs: MenuItem[] = [
    { label: 'profile', icon: 'pi pi-id-card', routerLink: '.' },
    { label: 'snippets', icon: 'pi pi-hashtag', routerLink: '.', queryParams: { tab: "snippets" } },
    { label: 'posts', icon: 'pi pi-at', routerLink: '.', queryParams: { tab: "posts" } },
    { label: 'comments', icon: 'pi pi-comment', routerLink: '.', queryParams: { tab: "comments" } },
    { label: 'likes', icon: 'pi pi-thumbs-up', routerLink: '.', queryParams: { tab: "likes" } },
  ];

  constructor(private usersService: UsersService,
              private route: ActivatedRoute,
              private router: Router) { }

  ngOnInit(): void {
    combineLatest([this.route.params, this.route.queryParams])
      .subscribe(([params, queryParams]) => {
        const userTag = params['user'];
        const tabValue = queryParams['tab'];
        const tabElement = this.tabs.find(tab => tab.label === tabValue);
        if (!tabElement) {
          this.tab = { label: 'profile', icon: 'pi pi-id-card', routerLink: '.' };
        } else {
          this.tab = tabElement;
        }
        this.getUser(userTag);
      });
  }

  private getUser(tag: string): void {
    this.usersService.getUser(tag).subscribe({
      next: (response) => {
        this.user = response;
      },
      error: (error) => {
        console.error("Error while fetching user data: ", error);
      }
    });
  }
}