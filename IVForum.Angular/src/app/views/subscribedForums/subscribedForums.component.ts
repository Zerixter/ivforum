import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { ForumService } from '../../services/forum.service';
import { Router } from '@angular/router';

@Component({
    selector: 'subscribedForums',
    templateUrl: 'subscribedForums.component.html',
    styleUrls: ["subscribedForums.component.css"]
})

export class SubscribedForumsComponent implements OnInit {
    private subscribedForums;
    constructor(
        private _userService:UserService,
        private _forumService:ForumService,
        private _router:Router
    ) { }

    ngOnInit() {
        this.getForums();
     }

    selectForum(forum){
        this._forumService.selectForum(forum);
        this._router.navigate(["/forum"]);
    }

    createForum(){
        this._router.navigate(["main/createForum"]);
    }

    getForums(){
        this._forumService.getSubscribedForums(JSON.parse(localStorage.getItem("currentUser")).token.id)
        .subscribe(
            res => this.subscribedForums = res,
            err => console.log(err)
        );
    }
}