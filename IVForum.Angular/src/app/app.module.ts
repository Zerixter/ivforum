import { CreateProjectComponent } from './views/createProject/createProject.component';
import { SubscribedForumsComponent } from './views/subscribedForums/subscribedForums.component';
import { BaseService } from './services/base.service';
import { ForumComponent } from './views/forum/forum.component';
import { MyProjectsComponent } from './views/myProjects/myProjects.component';
import { MyForumsComponent } from './views/myForums/myForums.component';
import { ProjectComponent } from './views/project/project.component';
import { LoginComponent } from './views/login/login.component';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';


import { AppComponent } from './app.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { appRouting } from './app.navigator';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RegisterComponent } from './views/register/register.component';
import { ExplorerComponent } from './views/explorer/explorer.component';
import { NavComponent } from './views/latNav/nav.component';
import { ForumService } from './services/forum.service';
import { UserService } from './services/user.service';
import { ProjectService } from './services/project.service';
import { SubscriptionService } from './services/subscription.service';
import { TransactionService } from './services/transaction.service';
import { AuthInterceptor } from './services/http-interceptor.service';
import { AuthGuard } from './services/auth-guard.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { MzCardModule,MzTabModule } from 'ng2-materialize';
import { MatTabsModule } from '@angular/material/tabs';
import { MatDialogModule } from '@angular/material/dialog';

import { MzButtonModule } from 'ng2-materialize';

import { MzModalModule } from 'ng2-materialize';

import { MzSpinnerModule } from 'ng2-materialize';
import { LoadService } from './services/load.service';
import { ForumsComponent } from './views/forums/forums.component';
import { CreateForumComponent } from './views/createForum/createForum.component';
import { MatFormFieldModule } from '@angular/material';
import { MatInputModule } from '@angular/material';

import { MzToastModule } from 'ng2-materialize';

import { MzDatepickerModule } from 'ng2-materialize';
import { FooterComponent } from './views/footer/footer.component';
import { AplicationComponent } from './views/aplication/aplication.component';
import { DialogOverviewExampleComponent } from './views/dialog-overview-example/dialog-overview-example.component';
import { MAT_DIALOG_DEFAULT_OPTIONS, MatFormField } from '@angular/material';
import { DialogOverviewExampleDialogComponent } from './views/dialog-overview-example-dialog/dialog-overview-example-dialog.component';
import { SelectProjectComponent } from './views/selectedProject/selectProject.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    ExplorerComponent,
    ProjectComponent,
    MyForumsComponent,
    MyProjectsComponent,
    NavComponent,
    ForumComponent,
    SubscribedForumsComponent,
    ForumsComponent,
    CreateForumComponent,
    FooterComponent,
    CreateProjectComponent,
    AplicationComponent,
    ProjectComponent,
    DialogOverviewExampleComponent,
    DialogOverviewExampleDialogComponent,
    SelectProjectComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    NoopAnimationsModule,
    MatFormFieldModule,
    MatInputModule,
    MatTabsModule,
    MatDialogModule,
    FormsModule,
    appRouting,
    HttpClientModule,
    ReactiveFormsModule,
    MzButtonModule,
    MzCardModule,
    MzTabModule,
    MzSpinnerModule,
    MzDatepickerModule,
    MzModalModule,
    MzToastModule
  ],
  providers: [
    ForumService,
    UserService,
    ProjectService,
    SubscriptionService,
    BaseService,
    TransactionService,
    AuthInterceptor,
    AuthGuard,
    LoadService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    {provide: MAT_DIALOG_DEFAULT_OPTIONS, useValue: {hasBackdrop: false}}
  ],
  entryComponents: [
    SelectProjectComponent
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
