import { UserDetailsService } from './services/user-details-service';
import { FooterComponent } from './views/shared/footer/footer.component';
import { appRouting } from './app-navigation.module';
import { NgForOf } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ApplicationModule } from '@angular/core';
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
import { ForumComponent } from './views/forum/forum.component';
import { TabViewModule } from 'primeng/tabview';
import { ProyectoService } from './services/proyecto.service';
import { ButtonModule } from 'primeng/button';
import { WalletService } from './services/wallet.service';
import { ScrollPanelModule } from 'primeng/scrollPanel';
import { SharedModule } from 'primeng/primeng';
import {MessageService} from 'primeng/components/common/messageservice';
import {MessagesModule} from 'primeng/messages';
import { MyForumsComponent } from './views/myForums/myForums.component';
import { ApplicationComponent } from './views/application/application.component';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    RegisterModal,
    LoginModal,
    HomeComponent,
    FooterComponent,
    ExplorerComponent,
    ForumComponent,
    MyForumsComponent,
    ApplicationComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    appRouting,
    HttpClientModule,
    ReactiveFormsModule,
    TabViewModule,
    ButtonModule,
    SharedModule,
    ScrollPanelModule,
    MessagesModule
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
    UserDetailsService,
    ProyectoService,
    WalletService,
    MessageService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
