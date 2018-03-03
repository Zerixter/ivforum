import { ForumComponent } from './views/forum/forum.component';
import { ExplorerComponent } from './views/explorar/explorer.component';
import { LoginModal } from './views/shared/header/login/login.component';
import { RegisterModal } from './views/shared/header/register/register.component';
import { Routes, RouterModule, CanActivate } from '@angular/router';
import { NgModule } from '@angular/core';
import { HomeComponent } from './views/home/home-body.component';
import { PageNotFoundComponent } from './views/notFound/pageNotFoundComponent.component';
import { AuthGuard } from './services/auth.guard';




const routes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'register', component: RegisterModal},
    { path: 'login', component: LoginModal},
    { path: 'explorer', component: ExplorerComponent},
    { path: 'forum', component: ForumComponent },
    { path: '**', component: HomeComponent },
    //{ path: 'path/:routeParam', component: MyComponent },
    //{ path: 'staticPath', component: ... },
    //{ path: '**', component: ... }, canActivate: [AuthGuard]
    //{ path: 'oldPath', redirectTo: '/staticPath' },
    //{ path: ..., component: ..., data: { message: 'Custom' }
];

export const appRouting = RouterModule.forRoot(routes);