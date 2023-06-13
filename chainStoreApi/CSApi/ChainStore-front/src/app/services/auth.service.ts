import { Injectable } from '@angular/core';
import { SessionStorageService } from 'angular-web-storage';
import { LogIn } from '../models/LogIn';
import { Person } from '../models/Person';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private sessionStorage: SessionStorageService) { }
  public isAuthenticated(): boolean {
    if (this.sessionStorage.get('user_loggedIn')) {
      return true;
    }
    else {
      return false;
    }
  }

  public getCurrentUser(): Person {
    return this.sessionStorage.get('user_loggedIn');
  }

  public setCurrentUser(userToSet: Person): void {
    if (!userToSet) return;
    this.sessionStorage.set('user_loggedIn', userToSet);
  }

  public clearCurrentUser(): void {
    this.sessionStorage.remove('user_loggedIn');
  }
}
