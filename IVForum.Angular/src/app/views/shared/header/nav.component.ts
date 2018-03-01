import { UserDetailsService } from './../../../services/user-details-service';
import { UserService } from './../../../services/users.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GlobalEventsManager } from '../../../services/globalEvents.service';

@Component({
    selector: 'navComponent',
    templateUrl: 'nav.component.html',
    styleUrls: ['nav.component.css']
})

export class NavComponent implements OnInit {
    private logged:boolean;
    private userDetails;
    private loaded: boolean = false;
    constructor(
        private _userService: UserService,
        private globalEventsManager: GlobalEventsManager,
        private router: Router,
        private _userDetailsService: UserDetailsService
    ) {
        if(this._userService.islogged()){
            this.logged = true;
            console.log("logged: "+this.logged);
        }
    }
    ngOnInit() {
        this.globalEventsManager.showNavBarEmitter.subscribe((mode)=>{
            this.logged = mode;
        });
        if(this._userService.islogged()){
            this.getDetails();
        }
        
    }

    getDetails() {
        this._userDetailsService.getUserDetails()
        .subscribe(res => {this.userDetails = res; this.loaded = true});
    }

    logOut(){
        this._userService.logout();
        this.router.navigateByUrl('login');
        this.logged = false;
    }
}