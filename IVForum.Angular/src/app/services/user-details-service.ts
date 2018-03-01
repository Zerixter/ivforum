import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class UserDetailsService {

    constructor(private http: HttpClient) { }

    public getUserDetails() {
        return this.http.get("http://localhost:57570/api/account/get")
            .map(
                res => res,
                err => {
                    console.log(err);
                }
            );
    }
    
}