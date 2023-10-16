import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {

  constructor(private router: Router) { }

  isAuthenticated() {
    const user =  localStorage.getItem("user");
    return user ? true : false;
  }

  isAdmin(): boolean {
    const user = JSON.parse(localStorage.getItem("user")!);
    return user ? user.role == "Admin" : false;
  }

  getUserId(): number {
    const user = JSON.parse(localStorage.getItem("user")!);
    return user.userId;
  }

  logout() {
    localStorage.removeItem("user");
    this.router.navigate(['/home']);
  }
}
