import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/users.service';
import { ProyectoService } from '../../services/proyecto.service';
import { ForumService } from '../../services/forum.service';
import { Router } from '@angular/router';

@Component({
    selector: 'myForums',
    templateUrl: 'myForums.component.html'
})

export class MyForumsComponent implements OnInit {
    private forums;

    constructor(
        private _usersService: UserService,
        private _projectService: ProyectoService,
        private _forumService: ForumService,
        private _router: Router,
    ) { }

    ngOnInit() {
        this.getForums();
     }

    getForums() {
        this._forumService.myForums(JSON.parse(localStorage.getItem("currentUser")).token.id)
            .subscribe(
                res => {
                    console.log(res);
                    this.forums = res;
                    console.log(this.forums);
                },
                err => console.log(err)
            );
    }
    selectForum(forum) {
        this._forumService.setSelectForum(forum);
        this._router.navigateByUrl("/forum");
    }
}