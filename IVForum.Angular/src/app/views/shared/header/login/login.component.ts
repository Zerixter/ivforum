import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { UserService } from '../../../../services/users.service';
import { Router } from '@angular/router';

@Component({
    selector: 'loginModal',
    templateUrl: 'login.component.html'
})

export class LoginModal implements OnInit {
    loginForm: FormGroup;

    email:string;
    password: string;
    
    constructor(
        private _userService: UserService,
        private router: Router
    ) { 
        
    }
    
    ngOnInit() {
        this.loginForm = new FormGroup({
            'email': new FormControl(null, [Validators.required,Validators.pattern("[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,3}$")]),
            'password': new FormControl(null,[Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*(_|[^\w])).+$'),Validators.minLength(8)])
        });
     }

    onSubmit(){
        //this._userService.
    }
}