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

    myForums(idUser:string) {
        return this.http.get("http://localhost:57570/api/forum/get/"+idUser)
            .map(
                res => console.log(res),
                err => console.log(err)
            );
    }

    setSelectForum(forum) {
        this.selectedForum = forum;
    }

    getSelectedForum() {
        return this.selectedForum;
    }

    asignProject(forumId,projectId) {
        return this.http.post("http://localhost:57570/api/forum/subscribe",{
            forumId:forumId,
            projectId:projectId
        })
            .map(
                res => res,
                err => console.log(err)
            );
    }
}