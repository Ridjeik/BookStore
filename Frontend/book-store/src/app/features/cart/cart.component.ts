import { Component, OnInit } from '@angular/core';
import { CartService } from '../../services/cart.service';
import { CommonModule } from '@angular/common';
import { BookService } from '../../services/book.service';
import { Book } from '../../types/book';
import { ToastModule } from 'primeng/toast';
import { RippleModule } from 'primeng/ripple';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, ToastModule, RippleModule],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.scss'
})
export class CartComponent implements OnInit {
  cartItems: { book: Book; quantity: number }[] = [];
  total: number = 0;
  discount: number = 0;
  totalWithoutDiscount: number = 0;

  constructor(
    private cartService: CartService,
    private bookService: BookService,
    private messageService: MessageService
  ) {}

  ngOnInit(): void {
    this.loadCartBooks();
  }

  loadCartBooks(): void {
    this.total = 0;
    this.discount = 0;
    this.totalWithoutDiscount = 0;

    const cartItems = this.cartService.getCartItems();
    this.cartItems = cartItems.map(item => {
      const book = this.bookService.getBookById(item.itemId);
      return { book: book!, quantity: item.quantity };
    });

    this.cartItems.forEach(({ book, quantity }) => {
      this.totalWithoutDiscount += book.price * quantity;
      if (book.discount) {
        this.discount += (book.price * book.discount / 100) * quantity;
      }
    });
    this.total = this.totalWithoutDiscount - this.discount;
  }

  removeFromCart(bookId: number): void {
    this.cartService.removeItem(bookId);
    this.loadCartBooks();
    this.messageService.add({severity: 'success', summary: 'Success', detail: 'Book removed from cart'});
  }

  clearCart(): void {
    this.cartService.clearCart();
    this.messageService.add({severity: 'success', summary: 'Success', detail: 'Cart cleared'});
    this.cartItems = [];
    this.total = 0;
    this.discount = 0;
    this.totalWithoutDiscount = 0;
  }

  updateQuantity(bookId: number, newQuantity: number): void {
    if (newQuantity > this.bookService.getBookById(bookId)!.copiesAvailable) {
      return;
    }

    if (newQuantity > 0) {
      this.cartService.addItem(bookId, newQuantity - this.cartItems.find(item => item.book.id === bookId)!.quantity);
      this.loadCartBooks();
    } else {
      this.removeFromCart(bookId);
    }
  }
}

