import { Injectable } from '@angular/core';
 
@Injectable()
export class ConfigService {
     
    _apiURI : string;
 
    constructor() {
        this._apiURI = 'localhost:60758/api';
     }
 
     getApiURI() {
         return this._apiURI;
     }    
}