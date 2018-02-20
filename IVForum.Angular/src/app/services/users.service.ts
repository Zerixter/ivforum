import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { BaseService } from "./base.service";

import { Observable } from 'rxjs/Rx';
import { BehaviorSubject } from 'rxjs/Rx';

// Add the RxJS Observable operators we need in this app.

import { UserRegistration } from '../interfaces/user-register.interface';
import { ConfigService } from './config.service';

@Injectable()

export class UserService extends BaseService {

    baseUrl: string = '';

    // Observable navItem source
    private _authNavStatusSource = new BehaviorSubject<boolean>(false);
    // Observable navItem stream
    authNavStatus$ = this._authNavStatusSource.asObservable();

    private loggedIn = false;

    constructor(private http: HttpClient, private configService: ConfigService) {
        super();
        this.loggedIn = !!localStorage.getItem('auth_token');
        // ?? not sure if this the best way to broadcast the status but seems to resolve issue on page refresh where auth status is lost in
        // header component resulting in authed user nav links disappearing despite the fact user is still logged in
        this._authNavStatusSource.next(this.loggedIn);
        this.baseUrl = this.baseUrl = configService.getApiURI();
    }

    register(mail: string, contraseña: string, nom: string, cognom: string) {
        let body = JSON.stringify({ nom, cognom, mail, contraseña });
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });
        console.log("patata");
        return this.http.post("http://localhost:57570/api/accounts", {
            name: nom,
            surname: cognom,
            email: mail,
            password: contraseña
        })
            .subscribe(
                res => {
                    console.log(res);
                },
                err => {
                    console.log(err);
                }
            );
    }

    login(userName, password) {
        let headers = new Headers();
        headers.append('Content-Type', 'application/json');

        return this.http.post("http://localhost:57570/api/auth", {
            email: userName,
            password: password
        })
            .subscribe(
                res => {
                    console.log(res);
                    /*localStorage.setItem('auth_token', res.);
                    this.loggedIn = true;
                    this._authNavStatusSource.next(true);*/
                    return true;
                },
                err => {
                    console.log(err);
                }
            )
    }

    logout() {
        localStorage.removeItem('auth_token');
        this.loggedIn = false;
        this._authNavStatusSource.next(false);
    }

    isLoggedIn() {
        return this.loggedIn;
    }
}