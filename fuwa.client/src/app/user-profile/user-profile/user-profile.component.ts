import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { User } from 'src/app/models/user';
import { UsersService } from 'src/app/services/users.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent {
  user: User | undefined;

  constructor(private usersService: UsersService,
              private route: ActivatedRoute
              ) { }

    ngOnInit(): void {
      this.route.params.subscribe(params => {
        const userTag = params['user'];
        this.getUser(userTag);
      })
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
