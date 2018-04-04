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
        console.log(forum);
        console.log(this._URL);
        return this.http.post(this._URL + "subscription/subscribe/forum",{id:forum.id})
        .map(
                res => {
                    console.log("subscrit");
                    return res;
                },
                err => {
                    console.log(err)
                    return false;
                });
    }

    subscribeProject(idForum,idProject){
        console.log("forum: " + idForum);
        return this.http.post(this._URL + "subscription/subscribe/project", {forumId:idForum,projectId:idProject})
        .map(
                res => {
                    return res;
                },
                err => {
                    console.log(err)
                    return false;
                });
    }

    isSubscribed(idForum){
        return this.http.get(this._URL + "account/subscribed/" + idForum)
        .map(
            res => {
                return true;
            },
            err => {
                console.log(err);
                console.log("resposta: ");
                return false;
            }
        )
    }
}

/*
    - api/subscription/
    - POST
        - subscribe/forum : Subscribe un usuario a un forum
        - subscribe/project : Subscribe un proyecto a un forum
*/