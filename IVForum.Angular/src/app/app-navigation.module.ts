import { LoginModal } from './views/shared/header/login/login.component';
import { RegisterModal } from './views/shared/header/register/register.component';
import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { HomeComponent } from './views/home/home-body.component';
import { PageNotFoundComponent } from './views/notFound/pageNotFoundComponent.component';
import { AuthGuard } from './services/auth.guard';




const routes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'register', component: RegisterModal, canActivate: [AuthGuard]},
    { path: 'login', component: LoginModal},
    { path: '**', component: HomeComponent },
    //{ path: 'path/:routeParam', component: MyComponent },
    //{ path: 'staticPath', component: ... },
    //{ path: '**', component: ... },
    //{ path: 'oldPath', redirectTo: '/staticPath' },
    //{ path: ..., component: ..., data: { message: 'Custom' }
];

export const appRouting = RouterModule.forRoot(routes);