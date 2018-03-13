import { Router } from '@angular/router';
import { UserService } from './../../services/user.service';
import { Component, OnInit } from '@angular/core';
import { ForumService } from '../../services/forum.service';
import { Forum } from '../../interfaces/forum.interface';
import { NavComponent } from '../latNav/nav.component';
import { LoadService } from '../../services/load.service';

@Component({
    selector: 'explorerComponent',
    templateUrl: 'explorer.component.html',
    styleUrls: ["explorer.component.css"]
})

export class ExplorerComponent implements OnInit {

    constructor(
        
    ) { }

    ngOnInit() {
    }

}