import { Component } from '@angular/core';




@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'ChainStore-front';
}

@Component({
  selector: 'app-home',
  template: '<h1>ChainStore</h1>',
})
export class HomeComponent { }
