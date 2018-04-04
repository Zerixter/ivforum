import { BaseService } from './base.service';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class TransactionService {

    private _URL;
    constructor(
        private URL:BaseService,
        private http: HttpClient
    ) {
        this._URL = this.URL.getURL();
     }

    subscribeForum(idProject,vote){
        console.log(idProject,vote);
        return this.http.post(this._URL + "transaction/vote",{projectId:idProject,value:vote})
        .map(
                res => {
                    return res;
                },
                err => {
                    console.log(err)
                    return false;
                });
    }
}

/*
- api/transaction
    - POST
        - vote : Votar a un proyecto
*/