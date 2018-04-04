import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { ProjectService } from '../../services/project.service';
import { Router } from '@angular/router';
import { ForumService } from '../../services/forum.service';
import { TransactionService } from '../../services/transaction.service';
import { MzToastService } from 'ng2-materialize';

@Component({
    selector: 'projectComponent',
    templateUrl: 'project.component.html'
})

export class ProjectComponent implements OnInit {
    private project;
    private forum;
    private votes;
    constructor(
        private _userService:UserService,
        private _projectService:ProjectService,
        private _forumService:ForumService,
        private _transactionService:TransactionService,
        private _router:Router,
        private toastService: MzToastService,
    ) { }

    ngOnInit() {
        this.getProject();
        this.getSubscriptionsOptions();
        console.log(this.project);
    }

    getProject(){
        this.project = this._projectService.getSelectedProject();
        this.forum = this._forumService.getSelectedForum();
    }

    addView(){
        this._projectService.addView(this.project.id)
        .subscribe(
            res => {return true},
            err => console.log(err)
        )
    }

    showToast() {
        this.toastService.show('Vot acceptat!', 4000, 'green');
    }

    voteProject(vote){
        console.log(this.project);
        this._transactionService.subscribeForum(this.project.id,vote.name)
        .subscribe(
            res => {
                this.getSubscriptionsOptions();
                this.showToast();
            },
            err => console.log(err)
        )
    }

    getSubscriptionsOptions(){
        this._userService.subscriptions(this.forum.id)
        .subscribe(
            res => {this.votes = res; console.log(res)},
            err => console.log(err)
        )
    }
}