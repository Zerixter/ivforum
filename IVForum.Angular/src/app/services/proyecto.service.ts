import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class ProyectoService {
    constructor(private http: HttpClient) {
    }
    getProjects(filter) {
        this.http.get("http://localhost:57570/api/project/get",filter)
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

    getProjectForum(id) {
        this.http.get("http://localhost:57570/api/forum/get/"+id+"/projects")
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

    setProject(proyecto) {
        var body;
        return this.http.post("http://localhost:57570/api/project/create",proyecto)
            .map(
                res => {
                    console.log("Proyecto Enviado");
                    return true;
                },
                err => {
                    console.log(err);
                    return false;
                }
            );
    }

}