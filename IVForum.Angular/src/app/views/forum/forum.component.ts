import { UserService } from './../../services/user.service';
import { ForumService } from './../../services/forum.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SubscriptionService } from '../../services/subscription.service';
import { ProjectService } from '../../services/project.service';
import { MzToastService } from 'ng2-materialize';
import { MatDialog } from '@angular/material';
import { SelectProjectComponent } from '../selectedProject/selectProject.component';


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
        private _subscriptionService:SubscriptionService,
        private toastService: MzToastService,
        private _dialog: MatDialog
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
        console.log("subscrit: ");
        this._userService.isSubscribed(this.forum.id)
        .subscribe(
            res => {
                this.subscribed = true;
                console.log(this.subscribed);
            },
            err => this.subscribed = false
        );

    }

    addProject(){
        let dialogRef = this._dialog.open(SelectProjectComponent, {
            width: '450px',
            data: {}
          });
      
          dialogRef.afterClosed().subscribe(result => {
            
        });
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
            res => this.showToastSubscribe(),
            err => console.log(err)
        )
    }

    projectMoreInfo(project){
        this._projectService.selectProject(project);
        this._router.navigate(["/main/project"])
    }

    getUserProjects(){
        this._projectService.getUserProject(JSON.parse(localStorage.getItem("currentUser")).token.id)
        .subscribe(
            res => this.userProjects = res,
            err => console.log(err)
        )
    }

    showToastSubscribe() {
        this.toastService.show("T'has inscrit!", 4000, 'green');
        this.getProjects();
    }

    showToastAddProject() {
        this.toastService.show("Has afegit un projecte!", 4000, 'green');
    }

    addToForum(project){
        this._subscriptionService.subscribeProject(this.forum.id,project.id)
        .subscribe(
            res => {
                this.showToastAddProject();
            }
        )
    }
}