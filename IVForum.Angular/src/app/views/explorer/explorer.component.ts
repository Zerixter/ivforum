import { Router } from '@angular/router';
import { UserService } from './../../services/user.service';
import { Component, OnInit } from '@angular/core';
import { ForumService } from '../../services/forum.service';
import { Forum } from '../../interfaces/forum.interface';

@Component({
    selector: 'explorerComponent',
    templateUrl: 'explorer.component.html'
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
     }

    getForums() {
        this._forumService.getForums()
        .subscribe(
            ress => this.forums = ress,
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
}