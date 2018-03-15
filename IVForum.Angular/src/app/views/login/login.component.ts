import { Router } from '@angular/router';
import { UserService } from './../../services/user.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
    selector: 'loginComponent',
    templateUrl: 'login.component.html',
    styleUrls: ["login.component.css"]
})

export class LoginComponent implements OnInit {
    private loginForm: FormGroup;
    private email:string;
    private password:string;
    constructor(
        private _userService:UserService,
        private _router:Router
    ) { }

    ngOnInit() {
        this.checkLogin();
        this.loginForm = new FormGroup({
            'email': new FormControl(null, [Validators.required, Validators.minLength(2)]),
            'password': new FormControl(null, [Validators.required, Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*(_|[^\w])).+$/), Validators.minLength(8)])
        });
    }

    checkLogin(){
        if(localStorage.getItem("currentUser") != null){
            this._router.navigate([""]);
        }
    }

    onSubmit(){
        this.logIn();
    }

    logIn(){
        console.log(this.email);
        this._userService.postLogin(this.email,this.password)
        .subscribe(
            res => {
                this._router.navigate([""]);
                console.log(res)
            },
            err => console.log(err)
        );
    }
}