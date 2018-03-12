import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';
import { ProjectService } from '../../services/project.service';

@Component({
    selector: 'myProjectComponent',
    templateUrl: 'myProject.component.html'
})

export class MyProjectComponent implements OnInit {
    private projects;

    constructor(
        private _userService:UserService,
        private _projectService:ProjectService,
        private _router:Router
    ) { }

    ngOnInit() {

    }

    getMyProjects(){
        this._projectService.getProjects()
        .subscribe(
            res => this.projects = res,
            err => console.log(err)
        )
    }

    selectProject(project){
        if (this._projectService.selectProject(project)){
            this._router.navigate[("/project")];
        }
    }


    
}