import { UserService } from './../../../services/users.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
    selector: 'navComponent',
    templateUrl: 'nav.component.html'
})

export class NavComponent implements OnInit {
    constructor(
        private _userService: UserService,
        private router: Router
    ) { }

    private logged:boolean = false;
    ngOnInit() { 
        console.log("nav");
        if (this._userService.islogged()){
            this.logged = true;
        }else {
            this.logged = false;
        }
    }
    logOut(){
        this._userService.logout();
        this.router.navigateByUrl('login');
        this.logged = false;
    }
}