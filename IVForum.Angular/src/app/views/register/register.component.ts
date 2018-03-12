import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';
import { Register } from '../../interfaces/register.interface';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
    selector: 'registerComponent',
    templateUrl: 'register.component.html',
    styleUrls: ["register.component.css"]
})

export class RegisterComponent implements OnInit {
    private registerForm: FormGroup;
    private name:string;
    private surname:string;
    private password:string;
    private email:string;
    constructor(
        private _userService:UserService,
        private _router:Router
    ) { }

    ngOnInit() {
        this.checkLogin();
        this.registerForm = new FormGroup({
            'email': new FormControl(null, [Validators.required,Validators.pattern("[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,3}$")]),
            'name': new FormControl(null, [Validators.required,Validators.minLength(2),Validators.maxLength(12)]),
            'surname': new FormControl(null, [Validators.required,Validators.minLength(2),Validators.maxLength(12)]),
            'password': new FormControl(null,[Validators.required,Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*(_|[^\w])).+$/),Validators.minLength(8)])
        });
    }

    checkLogin(){
        if(localStorage.getItem("currentUser") != null){
            this._router.navigate(["/explorer"]);
        }
    }

    onSubmit(){
        this.register();
    }

    register(){
        this._userService.postRegister(this.name,this.surname,this.email,this.password)
        .subscribe(
            res => {this._router.navigate(["/login"])},
            err => console.log(err)
        )
    }
}