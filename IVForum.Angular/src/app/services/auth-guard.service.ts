import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  CanActivate,
  Router
} from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { UserService } from './user.service';


@Injectable()
export class AuthGuard implements CanActivate {
  constructor(
    private auth: UserService,
    private router: Router
  ) { }
  canActivate() {
    // handle any redirects if a user isn't authenticated
    if (!this.auth.islogged()) {
      // redirect the user
      this.router.navigate(['login']);
      return false;
    }
    return true;
  }
}