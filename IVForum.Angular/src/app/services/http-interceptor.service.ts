import { Injectable, Injector } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs/Rx';
import 'rxjs/add/observable/throw'
import 'rxjs/add/operator/catch';
import { UserService } from './user.service';
import { Router } from '@angular/router';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    
    intercept(req: HttpRequest<any>,
        next: HttpHandler): Observable<HttpEvent<any>> {
        var idToken
        try{
         idToken= localStorage.getItem("currentUser");
        }catch(err){
            console.log(err);
        }
        if (idToken) {
            console.log(JSON.parse(idToken).token.auth_token);
            const cloned = req.clone({
                headers: req.headers.set("Authorization",
                    "Bearer " + JSON.parse(idToken).token.auth_token)
            });
            return next.handle(cloned);
        }
        else {
            return next.handle(req);
        }
    }
}
