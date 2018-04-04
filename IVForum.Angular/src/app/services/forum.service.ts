import { BaseService } from './base.service';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class ForumService {
    private selectedForum;
    private _URL
    constructor(
        private URL:BaseService,
        private http: HttpClient
    ) {
        this._URL = this.URL.getURL();
    }

    getForums(){
        return this.http.get(this._URL + "forum")
        .map(
                res => {
                    return res;
                },
                err => {
                    console.log(err);
                });
    }

    getUserForums(idUser){
        return this.http.get(this._URL + "forum/"+idUser)
        .map(
                res => {
                    return res;
                },
                err => {
                    console.log(err);
                    return false;
                });
    }

    getSubscribedForums(idUser){
        return this.http.get(this._URL + "forum/subscribed/"+idUser)
        .map(
                res => {
                    return res;
                },
                err => {
                    console.log(err);
                    return false;
                });
    }

    getForum(idForum){
        return this.http.get(this._URL + "forum/select/"+idForum)
        .map(
                res => {
                    return res;
                },
                err => {
                    console.log(err);
                    return false;
                });
    }
    
    createForum(title,description,dateBeginsVote,dateEndsVote){
        return this.http.post(this._URL + "forum", {title,description,dateBeginsVote,dateEndsVote})
        .map(
                res => {
                    return res;
                },
                err => {
                    console.log(err);
                    return false;
                });
    }

    putForum(forum){
        return this.http.put(this._URL + "forum", forum)
        .map(
                res => {
                    return res;
                },
                err => {
                    console.log(err);
                    return false;
                });
    }

    addView(forum){
        return this.http.put(this._URL + "forum/view", forum)
        .map(
                res => {
                    return res;
                },
                err => {
                    console.log(err);
                    return false;
                });
    }

    deleteForum(forum) {
        return this.http.delete(this._URL + "forum", forum)
        .map(
                res => {
                    return res;
                },
                err => {
                    console.log(err);
                    return false;
                });
    }

    selectForum(forum){
        this.selectedForum = forum;
        return true;
    }

    getForumProjects(idforum){
        return this.http.get(this._URL + "forum/projects/" + idforum)
        .map(
            res => {
                return res;
            },
            err => {
                console.log(err);
                return false;
            }
        );
    }

    getSelectedForum(){
        return this.selectedForum;
    }
}

/*
    - api/forum/
    - GET
        - default : Obtener todos los forum
        - {id_usuario} : Obtener forums de un usuario
        - user/{id_usuario} : Lo mismo pero para APP
        - subscribed/{id_usuario} : Forums a los que està suscrito un usuario
        - select/{id_forum} : Dades de un forum concret
    - POST
        - default : Crear forum
    - PUT
        - default : Actualizar forum
        - view : Añadir visita al forum
    - DELETE
        - default : Borrar forum
*/  