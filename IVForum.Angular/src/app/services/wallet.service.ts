import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable()
export class WalletService {

    constructor(private http: HttpClient) { }

    public join(id) {
        return this.http.post("http://199.247.14.254:8080/api/account/subscribe", id)
            .map(
                res => res,
                err => {
                    console.log(err);
                }
            );
    }

    getBills(forumId){
        return this.http.get("http://199.247.14.254:8080/api/account/bills/"+ forumId)
            .map(
                res => res,
                err => {
                    console.log(err);
                }
            );
    }
}