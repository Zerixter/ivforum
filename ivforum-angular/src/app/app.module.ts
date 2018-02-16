import { HomeComponent } from './home/home-body.component';
import { RegisterModal } from './shared/header/register/register.component';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { HttpModule, XHRBackend } from '@angular/http';
import { AppComponent } from './app.component';
import { NavComponent } from './shared/header/nav.component';
import { FormsModule } from '@angular/forms';
import { LoginModal } from './shared/header/login/login.component';
import { UserService } from './services/users.service';
import { BaseService } from './services/base.service';
import { ConfigService } from './services/config.service';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    RegisterModal,
    LoginModal,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule
  ],
  providers: [
    UserService,
    ConfigService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
