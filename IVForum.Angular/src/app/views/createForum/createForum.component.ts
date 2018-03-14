import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { ForumService } from '../../services/forum.service';
import { Router } from '@angular/router';
import { LoadService } from '../../services/load.service';

@Component({
    selector: 'createForum',
    templateUrl: 'createForum.component.html',
    styleUrls: ["createForum.component.css"]
})

export class CreateForumComponent implements OnInit {
    private title:string;
    private description:string;
    private dataBeginsVote;
    private dataEndsVote;
    constructor(
        private _userService:UserService,
        private _forumService:ForumService,
        private _router:Router,
        private _loader:LoadService
    ) {
    }

    ngOnInit() {
        this.title = "patata";
        $('.datepicker').pickadate({
            selectMonths: true, // Creates a dropdown to control month
            selectYears: 15, // Creates a dropdown of 15 years to control year,
            today: 'Today',
            clear: 'Clear',
            close: 'Ok',
            closeOnSelect: false // Close upon selecting a date,
          });
    }

    createForo(){
        this._forumService.createForum(this.title,this.description,this.dataBeginsVote,this.dataEndsVote)
        .subscribe(
            res => this._router.navigate(["main/forums"]),
            err => console.log(err)
        )
    }
}