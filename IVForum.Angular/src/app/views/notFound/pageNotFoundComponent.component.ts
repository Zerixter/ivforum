import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'pageNotFoundComponent',
    template: `template: 
    <article class="template animated slideInRight">
      <h4>Inconceivable!</h4>
      <div>I do not think this page is where you think it is.</div>
    </article>`
})

export class PageNotFoundComponent implements OnInit {
    constructor() { }

    ngOnInit() { }
}