import { UserService } from './../../services/users.service';
import { Component, OnInit } from '@angular/core';
import { ForumService } from '../../services/forum.service';
import { Router } from '@angular/router';
import { ProyectoService } from '../../services/proyecto.service';
import { WalletService } from '../../services/wallet.service';

@Component({
    selector: 'forumComponent',
    templateUrl: 'forum.component.html',
    styleUrls: ['forum.component.css']
})

export class ForumComponent implements OnInit {
    
    private forum;
    private projects = [];
    constructor(
        private _usersService: UserService,
        private _projectService: ProyectoService,
        private _forumService: ForumService,
        private _router: Router,
        private _walletService: WalletService
    ) { }

    ngOnInit() {
        this.getForum();
        this.getProjects();
    }

    getProjects() {
        let id = this.forum.id;
        console.log(id);
        this._projectService.getProjectForum(id);
    }

    subscribe() {
        this._walletService.subscribe(this.forum.id);
    }

    getForum() {
        this.forum = this._forumService.getSelectedForum();
        if(this.forum == null){
            this._router.navigateByUrl("/explorer");
        }
    }
}