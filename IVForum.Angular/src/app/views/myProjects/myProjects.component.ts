import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';
import { ProjectService } from '../../services/project.service';
//import { MatDialog } from '@angular/material';
import { CreateProjectComponent } from '../createProject/createProject.component';
import { MzToastService } from 'ng2-materialize';

@Component({
    selector: 'myProjectsComponent',
    templateUrl: 'myProjects.component.html',
    styleUrls:["myProjects.component.css"]
})

export class MyProjectsComponent implements OnInit {
    private projects;

    constructor(
        private _userService:UserService,
        private _projectService:ProjectService,
        private _router:Router,
        private toastService: MzToastService,
        //private _dialog: MatDialog
    ) { }

    ngOnInit() {
        this.getMyProjects();
        //this.test();
    }

    getMyProjects(){
        this._projectService.getUserProject(JSON.parse(localStorage.getItem("currentUser")).token.id)
        .subscribe(
            res => this.projects = res,
            err => console.log(err)
        )
    }

    createForum(){
        this._router.navigate(["main/createForum"]);
    }

    selectProject(project){
        if (this._projectService.selectProject(project)){
            this._router.navigate[("/main/project")];
        }
    }

    deleteProject(project){
        this._projectService.deleteProject(project)
        .subscribe(
            res => {
                this.showToastDeleteProject();
                this.getMyProjects();
            },
            err => console.log(err)
        )
    }

    createProject(){
        this._router.navigate(["/main/createProject"])
        /*let dialogRef = this._dialog.open(CreateProjectComponent, {
            width: '450px',
            data: {}
          });
      
          dialogRef.afterClosed().subscribe(result => {
            console.log('The dialog was closed');
        });*/
    }

    showToastDeleteProject() {
        this.toastService.show("Has eliminat un projecte!", 4000, 'green');
    }

    test(){
        var forum = [{'title':"potato","shortDescription":"Potato","description":"Normally, both your asses would be dead as fucking fried chicken, but you happen to pull this shit while I'm in a transitional period so I don't wanna kill you, I wanna help you. But I can't give you this case, it don't belong to me. Besides, I've already been through too much shit this morning over this case to hand it over to your dumb ass."},
        {'title':"potato","shortDescription":"Potato","description":"Normally, both your asses would be dead as fucking fried chicken, but you happen to pull this shit while I'm in a transitional period so I don't wanna kill you, I wanna help you. But I can't give you this case, it don't belong to me. Besides, I've already been through too much shit this morning over this case to hand it over to your dumb ass."},
        {'title':"potato","shortDescription":"Potato","description":"Normally, both your asses would be dead as fucking fried chicken, but you happen to pull this shit while I'm in a transitional period so I don't wanna kill you, I wanna help you. But I can't give you this case, it don't belong to me. Besides, I've already been through too much shit this morning over this case to hand it over to your dumb ass."},
        {'title':"potato","shortDescription":"Potato","description":"Normally, both your asses would be dead as fucking fried chicken, but you happen to pull this shit while I'm in a transitional period so I don't wanna kill you, I wanna help you. But I can't give you this case, it don't belong to me. Besides, I've already been through too much shit this morning over this case to hand it over to your dumb ass."}];
        this.projects = forum;
    }
    
}