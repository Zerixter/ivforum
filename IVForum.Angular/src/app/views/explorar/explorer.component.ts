import { Forum } from './../../interfaces/forum.interface';
import { UserService } from './../../services/users.service';
import { ForumService } from './../../services/forum.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import {FormsModule} from '@angular/forms';
import { NgForOf } from '@angular/common';
import { Router } from '@angular/router';


@Component({
    selector: 'explorerComponent',
    templateUrl: 'explorer.component.html',
    styleUrls: ['explorer.component.css']
})

export class ExplorerComponent implements OnInit {
    forums;
    title:string;
    name:string;
    description:string;
    constructor(
        private _forumService: ForumService,
        private _usersService: UserService,
        private _router: Router
    ) { }

    ngOnInit() {
        this.getForums();
    }

    getForums() {
       this._forumService.getForums("").subscribe(res => this.forums = res);
    }

    selectForum(forum) {
        this._forumService.setSelectForum(forum);
        this._router.navigateByUrl("/forum");
    }

    setForum() {
        //console.log = localStorage.getItem('currentUser')
        this._forumService.setForum(this.title,this.name,this.description).subscribe(
            res => {
                if (res){this.getForums()};
            }
        );
    }
}