import { AplicationComponent } from './views/aplication/aplication.component';
import { CreateProjectComponent } from './views/createProject/createProject.component';
import { ProjectComponent } from './views/project/project.component';
import { CreateForumComponent } from './views/createForum/createForum.component';
import { MyForumsComponent } from './views/myForums/myForums.component';
import { MyProjectsComponent } from './views/myProjects/myProjects.component';
import { RegisterComponent } from './views/register/register.component';
import { Component } from '@angular/core';
import { LoginComponent } from './views/login/login.component';
import { Routes, RouterModule } from "@angular/router";
import { ExplorerComponent } from "./views/explorer/explorer.component";
import { AuthGuard } from './services/auth-guard.service';
import { ForumsComponent } from './views/forums/forums.component';
import { ForumComponent } from './views/forum/forum.component';


const routes: Routes = [
    { path: '', redirectTo: '/main/forums', pathMatch: 'full'},
    { path: 'main', component: ExplorerComponent, canActivate: [AuthGuard],
        children:[
            { path: 'forums',component: ForumsComponent,canActivate: [AuthGuard]},
            { path: 'myProjects',component: MyProjectsComponent,canActivate: [AuthGuard]},
            { path: 'forum',component: ForumComponent,canActivate: [AuthGuard]},
            { path: 'project',component: ProjectComponent,canActivate: [AuthGuard]},
            { path: 'createForum',component: CreateForumComponent,canActivate: [AuthGuard]},
            { path: 'createProject', component: CreateProjectComponent,canActivate: [AuthGuard]},
            { path: 'app', component: AplicationComponent,canActivate: [AuthGuard]}
        ]
    },
    { path: 'login', component: LoginComponent},
    { path: 'register', component: RegisterComponent},
    { path: 'explorer', component: ExplorerComponent},
    { path: '**',component: ExplorerComponent}
    //{ path: 'path/:routeParam', component: MyComponent },
    //{ path: 'staticPath', component: ... },
    //{ path: '**', component: ... }, canActivate: [AuthGuard]
    //{ path: 'oldPath', redirectTo: '/staticPath' },
    //{ path: ..., component: ..., data: { message: 'Custom' }
];

export const appRouting = RouterModule.forRoot(routes);