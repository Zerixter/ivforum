import { MessageService } from 'primeng/components/common/messageservice';
import { UserService } from './../../services/users.service';
import { Component, OnInit } from '@angular/core';
import { ForumService } from '../../services/forum.service';
import { Router } from '@angular/router';
import { ProyectoService } from '../../services/proyecto.service';
import { WalletService } from '../../services/wallet.service';

import { Message } from 'primeng/components/common/api';
import { SelectItem } from 'primeng/components/common/api';


@Component({
    selector: 'forumComponent',
    templateUrl: 'forum.component.html',
    styleUrls: ['forum.component.css']
})

export class ForumComponent implements OnInit {
    private msgs: Message[] = [];
    private forum;
    private projects;
    private title: string;
    private name: string;
    private description: string;
    private myProjects;
    constructor(
        private _usersService: UserService,
        private _projectService: ProyectoService,
        private _forumService: ForumService,
        private _router: Router,
        private _walletService: WalletService,
        private mesageService: MessageService
    ) { }

    ngOnInit() {
        this.getForum();
        this.getProjects();
        this.getBills();
    }

    getProjects() {
        this._projectService.getProjectForum(this.forum.id).subscribe(res => this.projects = res);
    }

    createProject() {
        this._projectService.setProject(this.title, this.name, this.description).subscribe(res => {

        });
    }

    subscrib() {
        this._walletService.join(this.forum)
            .subscribe(res => {
                this.show();
            });
    }

    show() {
        this.msgs.push({ severity: 'success', summary: 'Ara participas en el foro :)' });
    }

    participate(project) {
        this._forumService.asignProject(this.forum.id, project.id)
            .subscribe(
                res => this.getProjects()
            );
    }

    getBills(){
        this._walletService.getBills(this.forum.id)
            .subscribe(
                res => console.log(res)
            );
    }

    getForum() {
        this.forum = this._forumService.getSelectedForum();
        if (this.forum == null) {
            this._router.navigateByUrl("/explorer");
        }
    }

    getUser(project) {
        this._usersService.getInfoUser(project.ownerId)
            .subscribe(res => {
                console.log(res)
            },
                err => console.log(err)
            );
    }
    getMyProjects() {
        this._projectService.getProjectUser(JSON.parse(localStorage.getItem("currentUser")).token.id)
            .subscribe(res => {
                this.myProjects = res;
                console.log(this.myProjects);
            });
    }
}