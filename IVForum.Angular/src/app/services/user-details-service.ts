import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class UserDetailsService {

    constructor(private http: HttpClient) { }

    public getUserDetails() {
        return this.http.get("http://199.247.14.254:8080/api/account/get")
            .map(
                res => res,
                err => {
                    console.log(err);
                }
            );
    }
    
}