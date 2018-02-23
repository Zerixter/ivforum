import { UserService } from './../../../services/users.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GlobalEventsManager } from '../../../services/globalEvents.service';

@Component({
    selector: 'navComponent',
    templateUrl: 'nav.component.html'
})

export class NavComponent implements OnInit {
    constructor(
        private _userService: UserService,
        private globalEventsManager: GlobalEventsManager,
        private router: Router
    ) { }

    private logged:boolean;
    ngOnInit() { 
        this.globalEventsManager.showNavBarEmitter.subscribe((mode)=>{
            this.logged = mode;
        });
    }
    logOut(){
        this._userService.logout();
        this.router.navigateByUrl('login');
        this.logged = false;
    }
}