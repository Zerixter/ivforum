import { Component, OnInit, Inject } from '@angular/core';
import { ProjectService } from '../../services/project.service';
import { UserService } from '../../services/user.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ForumService } from '../../services/forum.service';
import { SubscriptionService } from '../../services/subscription.service';
import { MzToastService } from 'ng2-materialize';

@Component({
    selector: 'selectProjectComponent',
    templateUrl: './selectProject.component.html',
})
export class SelectProjectComponent implements OnInit {
    private forum;
    private projects;
    constructor(
        private _projectService:ProjectService,
        private _forumService:ForumService,
        private _subscriptionService:SubscriptionService,
        private _userService:UserService,
        public dialogRef: MatDialogRef<SelectProjectComponent>,
        private toastService: MzToastService,
        @Inject(MAT_DIALOG_DATA) public data: any,
    ) { }

    ngOnInit() {
        this.getMyProjects();
        this.getSelectedForum();
    }

    getMyProjects(){
        this._projectService.getUserProject(JSON.parse(localStorage.getItem("currentUser")).token.id)
        .subscribe(
            res => this.projects = res,
            err => console.log(err)
        )
    }

    getSelectedForum(){
        this.forum = this._forumService.getSelectedForum();
    }

    subscribeProject(project){
        this._subscriptionService.subscribeProject(this.forum.id,project.id)
        .subscribe(
            res => {
                this.showToastAddProject();
                this.closeDialog();
            },
            err => console.log(err)
        )
    }

    showToastAddProject() {
        this.toastService.show("Has afegit un projecte!", 4000, 'green');
    }

    closeDialog(){
        this.dialogRef.close();
    }


}