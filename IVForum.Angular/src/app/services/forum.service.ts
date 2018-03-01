import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import 'rxjs/add/operator/map';
import { Forum } from '../interfaces/forum.interface';

@Injectable()
export class ForumService {

    public selectedForum = null;

    constructor(private http: HttpClient) {
    }
    getForums(filter) {
        return this.http.get("http://localhost:57570/api/forum/get",filter)
        
        .map(
            res => res,
            /* {
                console.log(res);
                return res;
            },*/
            err => {
                console.log(err);
            }
        );
    }

    setForum(title:string, name:string, description:string) {
        var body;
        return this.http.post("http://localhost:57570/api/forum/create",{
            title:title,
            name:name,
            description:description
        })
            .map(
                res => {
                    console.log("Enviado");
                    return true;
                },
                err => {
                    console.log(err);
                    return false;
                }
            );
    }

    setSelectForum(forum) {
        this.selectedForum = forum;
    }

    getSelectedForum() {
        console.log(this.selectedForum);
        return this.selectedForum;
    }
}