import { FooterComponent } from './views/shared/footer/footer.component';
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
import { HttpClient, HttpHandler, HttpClientModule, HTTP_INTERCEPTORS  } from '@angular/common/http';
import { AuthInterceptor } from './services/http-interceptor.service';
import { AuthGuard } from './services/auth.guard';
import { GlobalEventsManager } from './services/globalEvents.service';
import { ExplorerComponent } from './views/explorar/explorer.component';
import { ForumService } from './services/forum.service';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    RegisterModal,
    LoginModal,
    HomeComponent,
    FooterComponent,
    ExplorerComponent
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
    {
    provide: HTTP_INTERCEPTORS,
    useClass: AuthInterceptor,
    multi: true
    },
    AuthGuard,
    ForumService,
    GlobalEventsManager,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
