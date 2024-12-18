import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BookService } from '../../services/book.service';
import { ReviewService } from '../../services/review.service';
import { AuthService } from '../../services/auth.service';
import { Book } from '../../types/book';
import { Review } from '../../types/review';
import { CartService } from '../../services/cart.service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-book-details',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    FormsModule,
    DatePipe
  ],
  templateUrl: './book-details.component.html',
  styleUrl: './book-details.component.scss'
})
export class BookDetailsComponent implements OnInit {
  currentBook: Book | undefined;
  reviews: Review[] = [];
  newReview = {
    rating: 5,
    text: ''
  };
  showReviewForm = false;

  constructor(
    private route: ActivatedRoute,
    private bookService: BookService,
    private reviewService: ReviewService,
    public authService: AuthService,
    private messageService: MessageService,
    private cartService: CartService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    if (id) {
      const bookId = parseInt(id);
      this.currentBook = this.bookService.getBookById(bookId);
      this.reviews = this.reviewService.getReviews(bookId);
    }
  }

  addToCart(event:Event, book: Book): void {

    event.stopPropagation();

    if (this.cartService.getCartItems().find(item => item.itemId === book.id)?.quantity === book.copiesAvailable) {
      this.messageService.add({ severity: 'info', summary: 'Information', detail: 'Sorry, no more copies available.' });
      return;
    }

    this.cartService.addItem(book.id);

    this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Book added to cart' });
  }

  submitReview(): void {
    if (!this.currentBook || !this.authService.isLoggedIn()) return;

    const userInfo = this.authService.getUserInfo();
    
    const review = this.reviewService.addReview({
      bookId: this.currentBook.id,
      userId: userInfo.id,
      username: userInfo.username,
      rating: this.newReview.rating,
      text: this.newReview.text
    });

    this.reviews = [review, ...this.reviews];
    this.newReview = { rating: 5, text: '' };
    this.showReviewForm = false;

    this.bookService.saveBooks();

    const id = this.route.snapshot.paramMap.get('id');

    if (id) {
      const bookId = parseInt(id);
      this.currentBook = this.bookService.getBookById(bookId);
      this.reviews = this.reviewService.getReviews(bookId);
    }
  }

  getRatingStars(rating: number): number[] {
    return Array(5).fill(0).map((_, index) => index < rating ? 1 : 0);
  }
}
