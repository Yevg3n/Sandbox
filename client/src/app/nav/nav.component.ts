import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit{
  model: any ={};
  loggedIn = false;
  
  constructor(private AccountService: AccountService) {}

  ngOnInit(): void {
    this.getCurrentUser();
  }

  // TODO: Temporary code. To be removed.
  getCurrentUser(){
    this.AccountService.currentUser$.subscribe({
      next: user => this.loggedIn = !!user,
      error: error => console.log(error)
    });
  }

  login(){
    this.AccountService.login(this.model).subscribe({
      next: response => {
        console.log(response);
        this.loggedIn = true;
      },
      error: error => console.log(error)
    })
  }

  logout(){
    this.AccountService.logout();
    this.loggedIn = false;
  }
}
