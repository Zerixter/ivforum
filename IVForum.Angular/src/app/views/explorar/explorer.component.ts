import { Forum } from './../../interfaces/forum.interface';
import { UserService } from './../../services/users.service';
import { ForumService } from './../../services/forum.service';
import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'explorerComponent',
    templateUrl: 'explorer.component.html',
    styleUrls: ['explorer.component.css']
})

export class ExplorerComponent implements OnInit {
    private forums;
    private newForum:Forum;
    Title:string;
    Name:string;
    Description:string;
    constructor(
        private _forumService: ForumService,
        private _usersService: UserService
    ) { }

    ngOnInit() {
        this.getForums();
        console.log(JSON.parse(localStorage.getItem("currentUser")).token.id);
    }

    getForums() {
        this.forums = this._forumService.getForums("");
    }

    setForum() {

        //console.log = localStorage.getItem('currentUser')
        this._forumService.setForum(this.newForum).subscribe(
            res => {
                if (res){this.getForums()};
            }
        );
    }
}