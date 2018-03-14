import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';
import { ProjectService } from '../../services/project.service';

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
        private _router:Router
    ) { }

    ngOnInit() {
        //this.getMyProjects();
        this.test();
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

    test(){
        var forum = [{'title':"potato","shortDescription":"Potato","description":"Normally, both your asses would be dead as fucking fried chicken, but you happen to pull this shit while I'm in a transitional period so I don't wanna kill you, I wanna help you. But I can't give you this case, it don't belong to me. Besides, I've already been through too much shit this morning over this case to hand it over to your dumb ass."},
        {'title':"potato","shortDescription":"Potato","description":"Normally, both your asses would be dead as fucking fried chicken, but you happen to pull this shit while I'm in a transitional period so I don't wanna kill you, I wanna help you. But I can't give you this case, it don't belong to me. Besides, I've already been through too much shit this morning over this case to hand it over to your dumb ass."},
        {'title':"potato","shortDescription":"Potato","description":"Normally, both your asses would be dead as fucking fried chicken, but you happen to pull this shit while I'm in a transitional period so I don't wanna kill you, I wanna help you. But I can't give you this case, it don't belong to me. Besides, I've already been through too much shit this morning over this case to hand it over to your dumb ass."},
        {'title':"potato","shortDescription":"Potato","description":"Normally, both your asses would be dead as fucking fried chicken, but you happen to pull this shit while I'm in a transitional period so I don't wanna kill you, I wanna help you. But I can't give you this case, it don't belong to me. Besides, I've already been through too much shit this morning over this case to hand it over to your dumb ass."}];
        this.projects = forum;
    }
    
}