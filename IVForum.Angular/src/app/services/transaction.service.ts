import { BaseService } from './base.service';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class TransactionService {

    constructor(
        private _URL:BaseService,
        private http: HttpClient
    ) { }

    subscribeForum(idProject){
        return this.http.post(this._URL + "transaction/vote",idProject)
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