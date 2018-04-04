import { Component } from '@angular/core';
import { LoadService } from './services/load.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';
  private show:boolean;
  constructor(
    private load: LoadService
  ){
    this.load.setLoad(true);
    this.show = this.load.getStatus();
  }
}
