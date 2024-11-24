import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private cartKey = 'cart';

  constructor() {}

  getCartItemIds(): number[] {
    const cart = localStorage.getItem(this.cartKey);
    return cart ? JSON.parse(cart) : [];
  }

  addItemId(itemId: number): void {
    const cart = this.getCartItemIds();
    if (!cart.includes(itemId)) {
      cart.push(itemId);
      localStorage.setItem(this.cartKey, JSON.stringify(cart));
    }
  }

  removeItemId(itemId: number): void {
    const cart = this.getCartItemIds();
    const updatedCart = cart.filter(id => id !== itemId);
    localStorage.setItem(this.cartKey, JSON.stringify(updatedCart));
  }

  clearCart(): void {
    localStorage.removeItem(this.cartKey);
  }

  getItemCount(): number {
    return this.getCartItemIds().length;
  }
}