import { Component, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';
import { animate, style, transition, trigger } from '@angular/animations';

@Component({
  selector: 'app-account',
  standalone: true,
  imports: [FormsModule, 
    CommonModule,
  ],
  templateUrl: './account.component.html',
  styleUrl: './account.component.scss',
  animations: [
    trigger('fadeInOut', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(-10px)' }),
        animate('200ms ease-out', style({ opacity: 1, transform: 'translateY(0)' }))
      ]),
      transition(':leave', [
        animate('200ms ease-in', style({ opacity: 0, transform: 'translateY(-10px)' }))
      ])
    ])
  ]
})
export class AccountComponent {
  @Output() close = new EventEmitter<void>();

  username = '';
  password = '';
  loginFailed = false;

  constructor(public authService: AuthService) {}

  get isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }

  get userInfo(): any {
    return this.authService.getUserInfo();
  }

  onSubmit(event: Event): void {
    event.preventDefault();
    const success = this.authService.login(this.username, this.password);
    if (success) {
      this.loginFailed = false;
      this.close.emit();
    } else {
      this.loginFailed = true;
    }
  }

  logout(): void {
    this.authService.logout();
    this.close.emit(); 
  }
}
