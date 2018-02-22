import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { BaseService } from "./base.service";

import { Observable } from 'rxjs/Rx';
import { BehaviorSubject } from 'rxjs/Rx';
import 'rxjs/add/operator/map';

// Add the RxJS Observable operators we need in this app.

import { UserRegistration } from '../interfaces/user-register.interface';
import { ConfigService } from './config.service';


@Injectable()

export class UserService extends BaseService {

    baseUrl: string = '';
    
    public token: string = null;

    // Observable navItem source

    constructor(private http: HttpClient, private configService: ConfigService) {
        super();
        const currentUser = JSON.parse(localStorage.getItem('currentUser'));
        this.token = currentUser && currentUser.token;
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
            .map(
                res => {
                    console.log("registro corecto");
                    return true;
                },
                err => {
                    console.log(err);
                    return false;
                }
            );
    }

    login(userName: string, password: string) {
        console.log("intenta");
        return this.http.post('http://localhost:57570/api/auth', { userName, password })
        .map(
                res => {
                    console.log("login correcto!");
                    this.setSession(res);
                    return true;
                },
                err => {
                    console.log(err)
                    return false;
                }
            )
    }

    private setSession(authResult) {
        console.log("setSession");
        this.token = authResult.auth_token;
        console.log(this.token);
        localStorage.setItem('currentUser', authResult.auth_token);
    }

    islogged(){
        if (this.token != null){
            return true;
        }
        else {
            return false;
        }
    }

    logout(): void {
        localStorage.removeItem('currentUser');
        
    }
}