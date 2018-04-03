import { Component, OnInit, Inject } from '@angular/core';
import { UserService } from '../../services/user.service';
import { ForumService } from '../../services/forum.service';
import { Router } from '@angular/router';
//import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material';
import { CreateForumComponent } from '../createForum/createForum.component';

@Component({
    selector: 'myForumsComponent',
    templateUrl: 'myForums.component.html',
    styleUrls: ["myForums.component.css"]
})

export class MyForumsComponent implements OnInit {
    private myForums;
    private shortDescription;
    constructor(
        private _userService:UserService,
        private _forumService:ForumService,
        private _router:Router,
        //private _dialog: MatDialog
    ) { }

    ngOnInit() {
        this.getMyForums();
        //this.test();

    }
    
    selectFOrum(forum){
        this._forumService.selectForum(forum);
        this._router.navigate(["/forum"]);
    }

    getMyForums(){
        this._forumService.getUserForums(JSON.parse(localStorage.getItem("currentUser")).token.id)
        .subscribe(
            res => this.myForums = res,
            err => console.log(err)
        )
    }

    createForum(){
        this._router.navigate(["main/createForum"]);
        /*let dialogRef = this._dialog.open(CreateForumComponent, {
            width: '450px',
            data: {}
          });
      
          dialogRef.afterClosed().subscribe(result => {
            console.log('The dialog was closed');
        });*/
    }

    test(){
        var forum = [{'title':"potato","shortDescription":"Potato","description":"Normally, both your asses would be dead as fucking fried chicken, but you happen to pull this shit while I'm in a transitional period so I don't wanna kill you, I wanna help you. But I can't give you this case, it don't belong to me. Besides, I've already been through too much shit this morning over this case to hand it over to your dumb ass."},
        {'title':"potato","shortDescription":"Potato","description":"Normally, both your asses would be dead as fucking fried chicken, but you happen to pull this shit while I'm in a transitional period so I don't wanna kill you, I wanna help you. But I can't give you this case, it don't belong to me. Besides, I've already been through too much shit this morning over this case to hand it over to your dumb ass."},
        {'title':"potato","shortDescription":"Potato","description":"Normally, both your asses would be dead as fucking fried chicken, but you happen to pull this shit while I'm in a transitional period so I don't wanna kill you, I wanna help you. But I can't give you this case, it don't belong to me. Besides, I've already been through too much shit this morning over this case to hand it over to your dumb ass."},
        {'title':"potato","shortDescription":"Potato","description":"Normally, both your asses would be dead as fucking fried chicken, but you happen to pull this shit while I'm in a transitional period so I don't wanna kill you, I wanna help you. But I can't give you this case, it don't belong to me. Besides, I've already been through too much shit this morning over this case to hand it over to your dumb ass."}];
        this.myForums = forum;
    }
}