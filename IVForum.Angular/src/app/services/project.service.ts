import { BaseService } from './base.service';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class ProjectService {
    private selectedProject;
    private _URL;
    constructor(
        private URL:BaseService,
        private http: HttpClient
    ) {
        this._URL = this.URL.getURL();
     }

    getProjects(){
        return this.http.get(this._URL + "project")
        .map(
                res => {
                    return res;
                },
                err => {
                    console.log(err)
                    return false;
                });
    }

    getUserProject(idUser){
        return this.http.get(this._URL + "project/" + idUser)
        .map(
                res => {
                    return res;
                },
                err => {
                    console.log(err)
                    return false;
                });
    }

    getProject(idProject){
        return this.http.get(this._URL + "project/select/" + idProject)
        .map(
                res => {
                    return res;
                },
                err => {
                    console.log(err)
                    return false;
                });
    }

    createProject(title,description,websiteUrl,repositoryUrl){
        return this.http.post(this._URL + "project",{title:title,description:description,websiteUrl:websiteUrl,repositoryUrl:repositoryUrl})
        .map(
                res => {
                    return res;
                },
                err => {
                    console.log(err)
                    return false;
                });
    }
    voteProject(idProject){
        return this.http.post(this._URL + "project/vote",idProject)
        .map(
                res => {
                    return res;
                },
                err => {
                    console.log(err)
                    return false;
                });
    }
    //Cambiar
    modifProject(project){
        return this.http.put(this._URL + "project",project)
        .map(
                res => {
                    return res;
                },
                err => {
                    console.log(err)
                    return false;
                });
    }
    addView(idProject){
        return this.http.put(this._URL + "project/view",idProject)
        .map(
                res => {
                    return res;
                },
                err => {
                    console.log(err)
                    return false;
                });
    }

    deleteProject(object){
        return this.http.delete(this._URL + "project/"+object.id)
        .map(
                res => {
                    return res;
                },
                err => {
                    console.log(err)
                    return false;
                });
    }

    selectProject(project){
        this.selectedProject = project;
        return true;
    }

    getSelectedProject(){
        return this.selectedProject;
    }
}

/*
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
*/