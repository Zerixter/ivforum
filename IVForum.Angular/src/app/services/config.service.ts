import { Injectable } from '@angular/core';
 
@Injectable()
export class ConfigService {
     
    _apiURI : string;
 
    constructor() {
        this._apiURI = 'http://199.247.14.254:8080/api';
     }
 
     getApiURI() {
         return this._apiURI;
     }    
}