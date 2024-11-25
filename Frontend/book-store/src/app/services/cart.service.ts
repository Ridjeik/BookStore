import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { Cart } from '../types/cart';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private cartKey = 'cart';

  constructor(
    private authService: AuthService,
  ) {}

  private getStoredCart(): Cart | null {
    const allCarts: Cart[] = JSON.parse(localStorage.getItem(this.cartKey) || '[]');
    const userId = this.authService.getUserInfo().id;
    return allCarts.find(cart => cart.userId === userId) || null;
  }

  private saveCart(cart: Cart): void {
    const allCarts: Cart[] = JSON.parse(localStorage.getItem(this.cartKey) || '[]');
    const updatedCarts = allCarts.filter(c => c.userId !== cart.userId);
    updatedCarts.push(cart);
    localStorage.setItem(this.cartKey, JSON.stringify(updatedCarts));
  }

  private initializeCart(): Cart {
    const userId = this.authService.getUserInfo().id;
    return { userId, items: [] };
  }

  getCartItems(): { itemId: number; quantity: number }[] {
    const cart = this.getStoredCart() || this.initializeCart();
    return cart.items;
  }

  addItem(itemId: number, quantity: number = 1): void {
    let cart = this.getStoredCart() || this.initializeCart();
    const existingItem = cart.items.find(item => item.itemId === itemId);

    if (existingItem) {
      existingItem.quantity += quantity;
    } else {
      cart.items.push({ itemId, quantity });
    }

    this.saveCart(cart);
  }

  removeItem(itemId: number): void {
    let cart = this.getStoredCart() || this.initializeCart();
    cart.items = cart.items.filter(item => item.itemId !== itemId);
    this.saveCart(cart);
  }

  clearCart(): void {
    const allCarts: Cart[] = JSON.parse(localStorage.getItem(this.cartKey) || '[]');
    const userId = this.authService.getUserInfo().id;
    const updatedCarts = allCarts.filter(cart => cart.userId !== userId);
    localStorage.setItem(this.cartKey, JSON.stringify(updatedCarts));
  }
}