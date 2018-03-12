import { Injectable } from '@angular/core';

@Injectable()
export class BaseService {
    _apiURI = this._apiURI = "http://199.247.14.254:8080/api/";

    constructor() {
    }

    getURL(){
        console.log(this._apiURI);
        return this._apiURI;
    }
}

/*
Cosas a saber : default significa que no hay que poner nada simplemente pasar api/account por ese metodo HTTP

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
- api/project/
    - GET
        - default : Obtener todos los proyectos
        - {id_usuario} : Obtener todos los proyectos de un usuario
        - user/{id_usuario} : Lo mismo per para APP
        - select/{id_proyecto} : Obtener datos de un forum concreto
    - POST
        - default : Crear proyecto
        - vote : Votar proyecto
    - PUT
        - default : Actualizar proyecto
        - view : Añadir visita al proyecto
    - DELETE
        - default : Borrar proyecto
- api/subscription/
    - POST
        - subscribe/forum : Subscribe un usuario a un forum
        - subscribe/project : Subscribe un proyecto a un forum
- api/transaction
    - POST
        - vote : Votar a un proyecto
*/