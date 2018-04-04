import { ProjectService } from './../../services/project.service';
import { Component, OnInit, Inject } from '@angular/core';
import { ProjectComponent } from '../project/project.component';
import { Router } from '@angular/router';
import { UserService } from '../../services/user.service';
//import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

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
        //public dialogRef: MatDialogRef<CreateProjectComponent>,
        //@Inject(MAT_DIALOG_DATA) public data: any
    ) { }

    ngOnInit() {

    }

    createProject(){
        this._projectService.createProject(this.title,this.description,this.websiteUrl,this.repositoryUrl)
        .subscribe(
            res => this._router.navigate(["/main/myProjects"]),
            err => console.log(err)
        );
    }
}