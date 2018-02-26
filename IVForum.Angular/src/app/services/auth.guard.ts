import { UserService } from './users.service';
import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  CanActivate,
  CanLoad,
  Router
} from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { GlobalEventsManager } from './globalEvents.service';


@Injectable()
export class AuthGuard implements CanActivate {
  constructor(
    private auth: UserService,
    private router: Router,
    private globalEventsManager: GlobalEventsManager,
  ) { }
  canActivate() {
    // handle any redirects if a user isn't authenticated
    if (this.auth.islogged()){
      return true;
    }
    return false;
  }
  canLoad() {
    this.globalEventsManager.showNavBarEmitter.subscribe((mode)=>{
      return true;
    });
    return false;
  }
}