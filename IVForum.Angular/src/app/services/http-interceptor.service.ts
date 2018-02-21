import { Injectable, Injector } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs/Rx';
import 'rxjs/add/observable/throw'
import 'rxjs/add/operator/catch';
import { UserService } from './users.service';
import { Router } from '@angular/router';


@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    intercept(req: HttpRequest<any>,
        next: HttpHandler): Observable<HttpEvent<any>> {

        const idToken = localStorage.getItem("currentUser");

        if (idToken) {
            const cloned = req.clone({
                headers: req.headers.set("Authorization",
                    "Bearer " + idToken)
            });
            console.log(cloned);
            return next.handle(cloned);
        }
        else {
            return next.handle(req);
        }
    }
}
