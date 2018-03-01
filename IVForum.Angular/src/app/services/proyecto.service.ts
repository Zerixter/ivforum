import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class ProyectoService {
    constructor(private http: HttpClient) {
    }
    getForums(filter) {
        this.http.get("http://localhost:57570/api/proyecto/get",filter)
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

    setForum(proyecto) {
        var body;
        return this.http.post("http://localhost:57570/api/proyecto/post",proyecto)
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