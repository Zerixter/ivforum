import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import 'rxjs/add/operator/map';

@Injectable()
export class ForumService {

    constructor(private http: HttpClient) {
    }
    getForums(filter) {
        this.http.get("http://localhost:57570/api/forum/get",filter)
        .subscribe(
            res => {
                console.log(res);
                return res;
            },
            err => {
                console.log(err);
            }
        );
    }

    setForum(forum) {
        var body;
        return this.http.post("http://localhost:57570/api/forum/post",forum)
            .map(
                res => {
                    console.log("Forum Enviado");
                    return true;
                },
                err => {
                    console.log(err);
                    return false;
                }
            );
    }
}