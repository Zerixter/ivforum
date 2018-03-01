import { UserService } from './../../services/users.service';
import { Component, OnInit } from '@angular/core';
import { ForumService } from '../../services/forum.service';
import { Router } from '@angular/router';

@Component({
    selector: 'forumComponent',
    templateUrl: 'forum.component.html',
    styleUrls: ['forum.component.css']
})

export class ForumComponent implements OnInit {
    
    private forum;

    constructor(
        private _usersService: UserService,
        private _forumService: ForumService,
        private _router: Router
    ) { }

    ngOnInit() {
        this.getForum();
     }

    getForum() {
        console.log(this._forumService.getSelectedForum());
        this.forum = this._forumService.getSelectedForum();
        if(this.forum == null){
            this._router.navigateByUrl("/explorer");
        }
    }
}