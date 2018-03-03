import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class ProyectoService {
    constructor(private http: HttpClient) {
    }
    getProjects(filter) {
        return this.http.get("http://localhost:57570/api/project/get",filter)
        .subscribe(
            res => {
                return res;
            },
            err => {
                console.log(err);
            }
        );
    }

    getProjectForum(idForum) {
        return this.http.get("http://localhost:57570/api/forum/get/"+idForum+"/projects")
        .map(
            res => {
                return res;
            },
            err => {
                console.log(err);
            }
        );
    }

    getProjectUser(idUser) {
        return this.http.get("http://localhost:57570/api/project/get/"+idUser)
        .map(
            res => {
                return res;
            },
            err => {
                console.log(err);
            }
        );
    }

    setProject(title:string,name:string,description:string) {
        var body;
        return this.http.post("http://localhost:57570/api/project/create",{
            title:title,
            name:name,
            description:description
        })
            .map(
                res => {
                    return true;
                },
                err => {
                    console.log(err);
                    return false;
                }
            );
    }

}