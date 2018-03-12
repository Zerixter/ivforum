import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { ProjectService } from '../../services/project.service';
import { Router } from '@angular/router';

@Component({
    selector: 'projectComponent',
    templateUrl: 'project.component.html'
})

export class ProjectComponent implements OnInit {
    private project;
    constructor(
        private _userService:UserService,
        private _projectService:ProjectService,
        private _router:Router
    ) { }

    ngOnInit() {
        this.getProject();
    }

    getProject(){
        this.project = this._projectService.getSelectedProject()
    }

    modifProject(){
        this._projectService.modifProject(this.project)
        .subscribe(
            res => console.log("modificat"),
            err => console.log(err)
        )
    }
    deleteProject(){
        this._projectService.deleteProject(this.project)
        .subscribe(
            res => {this._router.navigate[("/myProjects")]},
            err => console.log(err)
        )
    }
}