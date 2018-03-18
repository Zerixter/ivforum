import { ForumService } from './../../services/forum.service';
import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';
import { SubscriptionService } from '../../services/subscription.service';
import { ProjectService } from '../../services/project.service';


@Component({
    selector: 'forumComponent',
    templateUrl: 'forum.component.html',
    styleUrls: ["forum.component.css"]
})

export class ForumComponent implements OnInit {
    private subscribed:boolean;


    private forum;
    private projects;
    private userProjects;
    
    constructor(
        private _userService:UserService,
        private _forumService:ForumService,
        private _projectService:ProjectService,
        private _router:Router,
        private _subscriptionService:SubscriptionService
    ) {
     }

    ngOnInit() {
        this.getForum();
        this.isSubscribed();
        this.getProjects();
     }

    getForum() {
        console.log("patata");
        this.forum = this._forumService.getSelectedForum()
    }

    isSubscribed(){
        this._subscriptionService.isSubscribed(this.forum.id)
        .subscribe(
            res => this.subscribed = true,
            err => this.subscribed = false
        );
    }

    modifForum(){
        this._forumService.putForum(this.forum)
        .subscribe(
            res => {this._router.navigate(["/main/forum"])},
            err => console.log(err)
        )
    }

    deleteForum(){
        this._forumService.deleteForum(this.forum)
        .subscribe(
            res => {this._router.navigate(["/main/myForums"])},
            err => console.log(err)
        )
    }

    getProjects(){
        this._forumService.getForumProjects(this.forum.id)
        .subscribe(
            res => this.projects = res
        )
    }

    subscribe(){
        this._subscriptionService.subscribeForum(this.forum)
        .subscribe(
            res => console.log("T'has subscrit!"),
            err => console.log(err)
        )
    }

    getUserProjects(){
        this._projectService.getUserProject(JSON.parse(localStorage.getItem("currentUser")).token.id)
        .subscribe(
            res => this.userProjects = res,
            err => console.log(err)
        )
    }

    addToForum(project){
        this._subscriptionService.subscribeProject(this.forum.id,project.id)
        .subscribe(
            res => {
                console.log("Has afegit el teu projecte!");
                this.getProjects();
            }
        )
    }
}