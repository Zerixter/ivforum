
import { Component, OnInit } from '@angular/core';
import { UserService } from '../../../../services/users.service';
import { Router } from '@angular/router';
import { FormGroup, FormControl, Validators } from '@angular/forms';


@Component({
    selector: 'registerModal',
    templateUrl: 'register.component.html'
})

export class RegisterModal implements OnInit {
    constructor(
        private _userService: UserService,
        private router: Router
    ) { }
    registerForm: FormGroup;

    name:string;
    surname:string;
    password:string;
    password_confirm:string;
    email:string;

    ngOnInit() {
        this.registerForm = new FormGroup({
            'email': new FormControl(null, [Validators.required,Validators.pattern("[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,3}$")]),
            'name': new FormControl(null, [Validators.required,Validators.minLength(2),Validators.maxLength(12)]),
            'surname': new FormControl(null, [Validators.required,Validators.minLength(2),Validators.maxLength(12)]),
            'password': new FormControl(null,[Validators.required,Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*(_|[^\w])).+$/),Validators.minLength(8)])
        });
    }

    onSubmit(){
        if(this._userService.register(this.email,this.password,this.name,this.surname)){
            this.router.navigateByUrl('/login');
        }
    }
}