import { BaseService } from './base.service';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class SubscriptionService {
    private _URL;
    constructor(
        private URL:BaseService,
        private http: HttpClient
    ) {
        this._URL = this.URL.getURL();
     }

    subscribeForum(forum){
        return this.http.post(this._URL + "subscribe/forum",forum)
        .map(
                res => {
                    return res;
                },
                err => {
                    console.log(err)
                    return false;
                });
    }

    subscribeProject(idForum,idProject){
        return this.http.post(this._URL + "subscribe/forum", {idForum,idProject})
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
    - api/subscription/
    - POST
        - subscribe/forum : Subscribe un usuario a un forum
        - subscribe/project : Subscribe un proyecto a un forum
*/