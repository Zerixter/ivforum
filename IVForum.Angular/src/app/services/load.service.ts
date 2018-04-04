import { Injectable } from '@angular/core';

@Injectable()
export class LoadService {
    private show:boolean;
    constructor() { }

    setLoad(choise:boolean){
        this.show = choise;
    }

    getStatus(){
        return this.show;
    }
}