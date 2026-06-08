import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { APP_NAME } from './Core/constants/app.constants';
import { Footer } from './shared/layout/footer/footer';
import { Header } from './shared/layout/header/header';
import { Navbar } from './shared/layout/navbar/navbar';

@Component({
  selector: 'app-root',
  imports: [Footer, Header, Navbar, RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {
  readonly appName = APP_NAME;
}
