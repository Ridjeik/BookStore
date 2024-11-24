import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly USER_KEY = 'userInfo';

  private mockUsers = [
    { id: 1, username: 'admin', password: '1111', role: 'admin' },
    { id: 2, username: 'user', password: '1111', role: 'user' },
  ];

  login(username: string, password: string): boolean {
    const user = this.mockUsers.find(
      (u) => u.username === username && u.password === password
    );

    if (user) {
      localStorage.setItem(this.USER_KEY, JSON.stringify(user));
      return true;
    }

    return false;
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem(this.USER_KEY);
  }

  getUserInfo(): any {
    const userInfo = localStorage.getItem(this.USER_KEY);
    return userInfo ? JSON.parse(userInfo) : null;
  }

  logout(): void {
    localStorage.removeItem(this.USER_KEY);
  }
}
