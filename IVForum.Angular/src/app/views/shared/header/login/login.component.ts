import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { UserService } from '../../../../services/users.service';
import { Router } from '@angular/router';
import { GlobalEventsManager } from '../../../../services/globalEvents.service';

@Component({
    selector: 'loginModal',
    templateUrl: 'login.component.html'
})

export class LoginModal implements OnInit {
    loginForm: FormGroup;

    email: string;
    password: string;

    constructor(
        private _userService: UserService,
        private globalEventsManager: GlobalEventsManager,
        private router: Router
    ) {

    }

    ngOnInit() {
        this.loginForm = new FormGroup({
            'email': new FormControl(null, [Validators.required, Validators.minLength(2)]),
            'password': new FormControl(null, [Validators.required, Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*(_|[^\w])).+$/), Validators.minLength(8)])
        });
    }

    onSubmit() {
        this._userService.login(this.email, this.password)
            .subscribe(data => {
                if (this._userService.islogged()) {
                    console.log(this._userService.islogged() + " en login");
                    this.router.navigateByUrl('/');
                    this.onLoginSuccessfully();
                }
            }
            );
        console.log(this._userService.islogged());
    }

    private onLoginSuccessfully(): void {
        /* --> HERE: you tell the global event manger to show the nav bar */
        this.globalEventsManager.showNavBar(true);
        this.router.navigate(['']);
    }
}
