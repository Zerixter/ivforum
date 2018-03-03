import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable()
export class WalletService {

    constructor(private http: HttpClient) { }

    public join(id) {
        return this.http.post("http://localhost:57570/api/account/subscribe", id)
            .map(
                res => res,
                err => {
                    console.log(err);
                }
            );
    }
}