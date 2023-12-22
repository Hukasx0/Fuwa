import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeRoutingModule } from './home/home-routing.module';
import { DashboardRoutingModule } from './dashboard/dashboard-routing.module';
import { LoginRoutingModule } from './login/login-routing.module';
import { RegisterRoutingModule } from './register/register-routing.module';
import { UserProfileRoutingModule } from './user-profile/user-profile-routing.module';
import { PostsRoutingModule } from './posts/posts-routing.module';
import { UserSettingsRoutingModule } from './user-settings/user-settings-routing.module';
import { CodeSnippetRoutingModule } from './code-snippet/code-snippet-routing.module';
import { PostRoutingModule } from './post/post-routing.module';
import { NewPostRoutingModule } from './new-post/new-post-routing.module';
import { NewCodeSnippetRoutingModule } from './new-code-snippet/new-code-snippet-routing.module';

const routes: Routes = [
  {
    path: '',
    loadChildren: () => import('./home/home.module').then(m => m.HomeModule)
  },
  {
    path: 'dashboard',
    loadChildren: () => import('./dashboard/dashboard.module').then(m => m.DashboardModule)
  },
  {
    path: 'settings',
    loadChildren: () => import('./user-settings/user-settings.module').then(m => m.UserSettingsModule)
  },
  {
    path: 'login',
    loadChildren: () => import('./login/login.module').then(m => m.LoginModule)
  },
  {
    path: 'register',
    loadChildren: () => import('./register/register.module').then(m => m.RegisterModule)
  },
  {
    path: 'new',
    loadChildren: () => import('./new-code-snippet/new-code-snippet.module').then(m => m.NewCodeSnippetModule)
  },
  {
    path: 'posts',
    loadChildren: () => import('./posts/posts.module').then(m => m.PostsModule)
  },
  {
    path: 'posts/new',
    loadChildren: () => import('./new-post/new-post.module').then(m => m.NewPostModule)
  },
  {
    path: 'posts/:id',
    loadChildren: () => import('./post/post.module').then(m => m.PostModule)
  },
  {
    path: ':user',
    loadChildren: () => import('./user-profile/user-profile.module').then(m => m.UserProfileModule)
  },
  {
    path: ':user/:snippet',
    loadChildren: () => import('./code-snippet/code-snippet.module').then(m => m.CodeSnippetModule)
  }
]

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forRoot(routes),

    HomeRoutingModule,
    DashboardRoutingModule,
    UserSettingsRoutingModule,
    LoginRoutingModule,
    RegisterRoutingModule,
    NewCodeSnippetRoutingModule,
    PostsRoutingModule,
    NewPostRoutingModule,
    PostRoutingModule,
    UserProfileRoutingModule,
    CodeSnippetRoutingModule

  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
