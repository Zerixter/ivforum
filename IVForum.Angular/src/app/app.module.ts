import { appRouting } from './app-navigation.module';

import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule, XHRBackend } from '@angular/http';
import { AppComponent } from './app.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { UserService } from './services/users.service';
import { BaseService } from './services/base.service';
import { ConfigService } from './services/config.service';
import { LoginModal } from './views/shared/header/login/login.component';
import { NavComponent } from './views/shared/header/nav.component';
import { RegisterModal } from './views/shared/header/register/register.component';
import { HomeComponent } from './views/home/home-body.component';
import { HttpClient, HttpHandler, HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    RegisterModal,
    LoginModal,
    HomeComponent,
    
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    appRouting,
    HttpClientModule,
    ReactiveFormsModule
  ],
  providers: [
    UserService,
    ConfigService,
    HttpClient,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
