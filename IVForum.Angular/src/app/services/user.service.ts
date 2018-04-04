import { BaseService } from './base.service';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class UserService {

    private _URL;
    constructor(
        private URL:BaseService,
        private http: HttpClient
    ) {
        this._URL = this.URL.getURL();
     }

    postRegister(name:string, surname:string, email:string, password:string) {
        return this.http.post(this._URL + "account/register", {name,surname,email,password})
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

    postLogin(email: string, password: string) {
        console.log(email+","+password);
        return this.http.post(this._URL + "account/login", { email, password })
        .map(
                res => {
                    localStorage.setItem('currentUser', JSON.stringify({ email: email, token:res  }));
                    return true;
                },
                err => {
                    console.log(err)
                    return false;
                }
            )
    }

    getInfoUser(userId){
        return this.http.get(this._URL + "account" + userId)
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
        console.log("idforum: " + idForum);
        return this.http.get(this._URL + "subscription/subscribed/" + idForum)
        .map(
                res => {
                    console.log(res);
                    return true;
                },
                err => {
                    console.log(err)
                    return false;
                });
    }

    subscriptions(idForum){
        console.log("forum: " + idForum)
        return this.http.get(this._URL + "subscription/wallet/" + idForum)
        .map(
                res => {
                    console.log(res);
                    return res;
                },
                err => {
                    console.log(err);
                    return false;
                });
    }

    putUser(user){
        return this.http.put(this._URL + "account", user)
        .map(
                res => {
                    return res;
                },
                err => {
                    console.log(err)
                    return false;
                });
    }

    deleteUser(user){
        return this.http.delete(this._URL + "account", user)
        .map(
                res => {
                    return res;
                },
                err => {
                    console.log(err)
                    return false;
                });
    }

    islogged(){
        if (localStorage.getItem('currentUser') != null){
            return true;
        }
        else {
            return false;
        }
    }

    logout() {
        localStorage.removeItem('currentUser');
        return true;
    }
}

/*
- api/account/
    - GET
        - default: Obtener datos de un usuario concreto
        - {id_usuario} : Obtener datos del usuario
        - subscribed/{id_forum} : Si el usuario esta suscrito a ese forum
        - subscription/{id_forum} : Las opciones de voto que tiene el user en ese forum
    - POST
        - register : Registrar usuario
        - login : Loguear usuario
        - avatar : Cambiar imagen de perfil
    - PUT
        - default : Actualizar usuario
    - DELETE
        - default : Borrar usuario
*/