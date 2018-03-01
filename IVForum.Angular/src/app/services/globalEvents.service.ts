import { UserService } from './users.service';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";

@Injectable()
export class GlobalEventsManager {

    private _showNavBar: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
    public showNavBarEmitter: Observable<boolean> = this._showNavBar.asObservable();

    constructor(
        private _userService : UserService
    ) {}

    showNavBar(ifShow: boolean) {
        if (this._userService.islogged()){
            ifShow = true;
            console.log(ifShow);
        }
        this._showNavBar.next(ifShow);
    }
}