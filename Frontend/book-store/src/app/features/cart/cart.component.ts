import { Component, OnInit } from '@angular/core';
import { CartService } from '../../services/cart.service';
import { CommonModule } from '@angular/common';
import { BookService } from '../../services/book.service';
import { Book } from '../../types/book';
import { ToastModule } from 'primeng/toast';
import { Ripple, RippleModule } from 'primeng/ripple';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, ToastModule, RippleModule],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.scss'
})
export class CartComponent implements OnInit {
  cartItems: Book[] = [];

  public total: number = 0;
  public discount: number = 0;
  public totalWithoutDiscount: number = 0;

  constructor(private cartService: CartService,
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

    const cartItemIds = this.cartService.getCartItemIds();
    this.cartItems = this.bookService.getBooksByIds(cartItemIds);

    this.cartItems.forEach((book) => {
      this.totalWithoutDiscount += book.price;
      if (book.discount) {
        this.discount += book.price * book.discount / 100;
      }
    });
    this.total = this.totalWithoutDiscount - this.discount;
  }

  removeFromCart(bookId: number): void {
    console.log('removeFromCart', bookId);
    this.cartService.removeItemId(bookId);
    this.loadCartBooks();

    this.messageService.add({severity: 'success', summary: 'Success', detail: 'Book removed from cart'});
  }

  clearCart(): void {
    this.cartService.clearCart();

    this.messageService.add({severity: 'success', summary: 'Success', detail: 'Cart cleared'});

    this.cartItems = [];
  }
}