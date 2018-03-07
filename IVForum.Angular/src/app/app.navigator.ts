import { MyForumsComponent } from './views/myForums/myForums.component';
import { MyProjectComponent } from './views/myProjects/myProjects.component';
import { RegisterComponent } from './views/register/register.component';
import { Component } from '@angular/core';
import { LoginComponent } from './views/login/login.component';
import { Routes, RouterModule } from "@angular/router";
import { ExplorerComponent } from "./views/explorer/explorer.component";
import { AuthGuard } from './services/auth-guard.service';


const routes: Routes = [
    { path: '', component: ExplorerComponent},
    { path: 'login',component: LoginComponent},
    { path: 'register',component: RegisterComponent},
    { path: 'myForums',component: MyForumsComponent,canActivate: [AuthGuard]},
    { path: 'myProjects',component: MyProjectComponent,canActivate: [AuthGuard]},
    { path: '**',component: ExplorerComponent}
    //{ path: 'path/:routeParam', component: MyComponent },
    //{ path: 'staticPath', component: ... },
    //{ path: '**', component: ... }, canActivate: [AuthGuard]
    //{ path: 'oldPath', redirectTo: '/staticPath' },
    //{ path: ..., component: ..., data: { message: 'Custom' }
];

export const appRouting = RouterModule.forRoot(routes);