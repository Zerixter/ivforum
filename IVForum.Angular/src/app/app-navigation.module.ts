import { MyForumsComponent } from './views/myForums/myForums.component';
import { ForumComponent } from './views/forum/forum.component';
import { ExplorerComponent } from './views/explorar/explorer.component';
import { LoginModal } from './views/shared/header/login/login.component';
import { RegisterModal } from './views/shared/header/register/register.component';
import { Routes, RouterModule, CanActivate } from '@angular/router';
import { NgModule } from '@angular/core';
import { HomeComponent } from './views/home/home-body.component';
import { PageNotFoundComponent } from './views/notFound/pageNotFoundComponent.component';
import { AuthGuard } from './services/auth.guard';
import { ApplicationComponent } from './views/application/application.component';




const routes: Routes = [
    { path: '', redirectTo: '/login', pathMatch: 'full' },
    { path: 'register', component: RegisterModal},
    { path: 'login', component: LoginModal},
    { path: 'explorer', component: ExplorerComponent,canActivate: [AuthGuard]},
    { path: 'forum', component: ForumComponent ,canActivate: [AuthGuard]},
    { path: 'myForums', component: MyForumsComponent,canActivate: [AuthGuard]},
    { path: 'app', component: ApplicationComponent },
    { path: '**', component: HomeComponent },
    //{ path: 'path/:routeParam', component: MyComponent },
    //{ path: 'staticPath', component: ... },
    //{ path: '**', component: ... }, canActivate: [AuthGuard]
    //{ path: 'oldPath', redirectTo: '/staticPath' },
    //{ path: ..., component: ..., data: { message: 'Custom' }
];

export const appRouting = RouterModule.forRoot(routes);