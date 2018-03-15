import { ProjectService } from './../../services/project.service';
import { Component, OnInit } from '@angular/core';
import { ProjectComponent } from '../project/project.component';
import { Router } from '@angular/router';
import { UserService } from '../../services/user.service';

@Component({
    selector: 'createProject',
    templateUrl: 'createProject.component.html',
    styleUrls:["createProject.component.css"]
})

export class CreateProjectComponent implements OnInit {
    private title:string;
    private description:string;
    private websiteUrl:string;
    private repositoryUrl:string;
    constructor(
        private _userService:UserService,
        private _projectService:ProjectService,
        private _router:Router,
    ) { }

    ngOnInit() {

    }

    createProject(){
        this._projectService.createProject(this.title,this.description,this.websiteUrl,this.repositoryUrl)
        .subscribe(
            res => this._router.navigate(["/myProjects"]),
            err => console.log(err)
        );
    }
}