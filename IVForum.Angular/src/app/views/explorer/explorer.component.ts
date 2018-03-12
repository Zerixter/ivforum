import { Router } from '@angular/router';
import { UserService } from './../../services/user.service';
import { Component, OnInit } from '@angular/core';
import { ForumService } from '../../services/forum.service';
import { Forum } from '../../interfaces/forum.interface';
import { NavComponent } from '../latNav/nav.component';

@Component({
    selector: 'explorerComponent',
    templateUrl: 'explorer.component.html',
    styleUrls: ["explorer.component.css"]
})

export class ExplorerComponent implements OnInit {
    private forums;
    private newForum: Forum;
    constructor(
        private _userService:UserService,
        private _forumService:ForumService,
        private _router:Router
    ) { }

    ngOnInit() {
        //this.newForum = new Forum();
        this.getForums();
        this.test();
     }

    getForums() {
        this._forumService.getForums()
        .subscribe(
            res => this.forums = res,
            err => console.log(err) 
        )
    }

    getMyForums(){
        this._forumService.getUserForums(JSON.parse(localStorage.getItem("currentUser")).id)
        .subscribe(
            res => this.forums = res,
            err => console.log(err)
        )
    }

    createForum(){
        this._forumService.createForum(this.newForum)
        .subscribe(
            res => {
                console.log("suscrito");
            },
            err => console.log(err)
        )
    }
    test(){
        var forum = [{'title':"potato","shortDescription":"Potato","description":"Normally, both your asses would be dead as fucking fried chicken, but you happen to pull this shit while I'm in a transitional period so I don't wanna kill you, I wanna help you. But I can't give you this case, it don't belong to me. Besides, I've already been through too much shit this morning over this case to hand it over to your dumb ass."},
        {'title':"potato","shortDescription":"Potato","description":"Normally, both your asses would be dead as fucking fried chicken, but you happen to pull this shit while I'm in a transitional period so I don't wanna kill you, I wanna help you. But I can't give you this case, it don't belong to me. Besides, I've already been through too much shit this morning over this case to hand it over to your dumb ass."},
        {'title':"potato","shortDescription":"Potato","description":"Normally, both your asses would be dead as fucking fried chicken, but you happen to pull this shit while I'm in a transitional period so I don't wanna kill you, I wanna help you. But I can't give you this case, it don't belong to me. Besides, I've already been through too much shit this morning over this case to hand it over to your dumb ass."},
        {'title':"potato","shortDescription":"Potato","description":"Normally, both your asses would be dead as fucking fried chicken, but you happen to pull this shit while I'm in a transitional period so I don't wanna kill you, I wanna help you. But I can't give you this case, it don't belong to me. Besides, I've already been through too much shit this morning over this case to hand it over to your dumb ass."}];
        this.forums = forum;
    }
}