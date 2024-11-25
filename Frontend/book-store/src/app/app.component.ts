import { Component, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterOutlet } from '@angular/router';
import { AccountComponent } from "./features/account/account.component";
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { RippleModule } from 'primeng/ripple';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, 
    RouterOutlet, 
    RouterLink, 
    AccountComponent,
    ToastModule,
    RippleModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'book-store';

  constructor(
    public authService: AuthService) {}

  showPopup = false;

  togglePopup(): void {
    this.showPopup = !this.showPopup;
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    const clickedElement = event.target as HTMLElement;
    const popup = document.querySelector('app-account');
    const userIcon = document.querySelector('#account');

    if (!popup?.contains(clickedElement) && !userIcon?.contains(clickedElement)) {
      this.showPopup = false;
    }
  }
}
