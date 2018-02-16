import { UserService } from './../../../services/users.service';
import { Component, OnInit } from '@angular/core';
import { UserRegistration } from '../../../interfaces/user-register.interface';

@Component({
    selector: 'registerModal',
    templateUrl: 'register.component.html'
})

export class RegisterModal implements OnInit {
    constructor(
        public _userService: UserService
    ) { }
    name:string;
    surname:string;
    password:string;
    email:string;

    ngOnInit() { }

    onSubmit(){
        this._userService.register(this.email,this.password,this.name,this.surname);
    }
}