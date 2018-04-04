import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';

@Component({
    selector: 'navComponent',
    templateUrl: 'nav.component.html'
})

export class NavComponent implements OnInit {
    constructor(
        private _userService:UserService,
        private _router:Router
    ) { }

    sidebar_activated:boolean = false;

    ngOnInit() {

    }

    logout(){
        if(this._userService.logout()){
            this._router.navigate(["/login"]);
        }
    }
}