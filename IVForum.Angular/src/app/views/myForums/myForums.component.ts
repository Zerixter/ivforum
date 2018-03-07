import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { ForumService } from '../../services/forum.service';
import { Router } from '@angular/router';

@Component({
    selector: 'myForumsComponent',
    templateUrl: 'myForums.component.html'
})

export class MyForumsComponent implements OnInit {
    private forums;
    constructor(
        private _userService:UserService,
        private _forumService:ForumService,
        private _router:Router
    ) { }

    ngOnInit() {
        this.getMyForums();
    }

    getMyForums(){
        this._forumService.getUserForums(JSON.parse(localStorage.getItem("currentUser")).token.id)
        .subscribe(
            res => this.forums = res,
            err => console.log(err)
        )
    }

}