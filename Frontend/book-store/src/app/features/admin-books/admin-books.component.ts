import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { BookService } from '../../services/book.service';
import { ReviewService } from '../../services/review.service';
import { Book } from '../../types/book';
import { CommonModule, DatePipe } from '@angular/common';
import { Discount } from '../../types/discount';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { RippleModule } from 'primeng/ripple';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    ToastModule,
    RippleModule
  ],
  templateUrl: './admin-books.component.html',
  styleUrls: ['./admin-books.component.scss']
})
export class AdminBooksComponent implements OnInit {
  books: Book[] = [];
  reviews: any[] = [];
  discounts: Discount[] = [];
  bookForm: FormGroup;
  discountForm: FormGroup;
  selectedBook: Book | null = null;

  discountTypes = ['Book', 'Genre', 'Author'];
  discountValues: string[] = [];
  discountLabels: string[] = [];

  activeTab: 'books' | 'reviews' | 'discounts' = 'books';

  constructor(
    public bookService: BookService,
    private reviewService: ReviewService,
    private fb: FormBuilder,
    private messageService: MessageService,
    private datePipe: DatePipe
  ) {
    this.bookForm = this.fb.group({
      title: ['', Validators.required],
      author: ['', Validators.required],
      publisher: ['', Validators.required],
      publicationDate: [new Date(), Validators.required],
      pageCount: [1, [Validators.required, Validators.min(1)]],
      isbn: ['', Validators.required],
      description: ['', Validators.required],
      price: [0.01, [Validators.required, Validators.min(0.01)]],
      coverImage: ['', Validators.required],
      category: ['', Validators.required],
      copiesAvailable: [0, [Validators.required, Validators.min(0)]],
    });

    this.discountForm = this.fb.group({
      type: ['Book', Validators.required],
      value: ['', Validators.required],
      amount: [0, [Validators.required, Validators.min(0), Validators.max(100)]]
    });
  }

  ngOnInit(): void {
    this.loadBooks();
    this.loadReviews();
    this.loadDiscounts();
    this.updateDiscountValues();
  }

  loadBooks(): void {
    this.books = this.bookService.getBooks();
  }

  loadReviews(): void {
    this.reviews = this.reviewService.getAllReviews();
  }

  loadDiscounts(): void {
    const storedDiscounts = localStorage.getItem('discounts');
    this.discounts = storedDiscounts ? JSON.parse(storedDiscounts) : [];
  }

  saveDiscounts(): void {
    localStorage.setItem('discounts', JSON.stringify(this.discounts));
  }

  getBookTitle(id: number): string | undefined {
    return this.books.find(book => book.id === id)?.title;
  }

  onBookSubmit(): void {
    if (this.bookForm.valid) {
      const bookData = this.bookForm.value;
      bookData.publicationDate = new Date(bookData.publicationDate);
      if (this.selectedBook) {
        this.bookService.updateBook({ ...this.selectedBook, ...bookData });
        this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Book updated successfully'});
      } else {
        this.bookService.addBook(bookData);
        this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Book added successfully'});
      }
      this.loadBooks();
      this.resetBookForm();
    }
  }

  getDiscountValueLabel(discount: Discount): string | undefined{
    return discount.type === 'Book' ? this.bookService.getBookById(parseInt(discount.value))?.title : discount.value
  }

  editBook(book: Book): void {
    this.selectedBook = book;
    this.bookForm.patchValue({
      ...book,
      publicationDate: this.datePipe.transform(book.publicationDate, 'yyyy-MM-dd')
    });
  }

  deleteBook(id: number): void {
    this.bookService.deleteBook(id);
    this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Book deleted successfully'});
    this.loadBooks();
  }

  deleteReview(id: string): void {
    this.reviewService.deleteReview(id);
    this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Review deleted successfully'});
    this.loadReviews();
  }

  resetBookForm(): void {
    this.selectedBook = null;
    this.bookForm.reset();
  }

  updateDiscountValues(): void {
    const type = this.discountForm.get('type')?.value;
    switch (type) {
      case 'Book':
        this.discountValues = this.books.map(book => book.id.toString());
        this.discountLabels = this.books.map(book => book.title);
        break;
      case 'Genre':
        this.discountValues = Array.from(new Set(this.books.map(book => book.category)));
        this.discountLabels = this.discountValues;
        break;
      case 'Author':
        this.discountValues = Array.from(new Set(this.books.map(book => book.author)));
        this.discountLabels = this.discountValues;
        break;
    }
  }

  onDiscountSubmit(): void {
    if (this.discountForm.valid) {
      const discountData = this.discountForm.value;
      const newDiscount: Discount = {
        id: Date.now(),
        ...discountData
      };
      this.discounts.push(newDiscount);
      this.saveDiscounts();
      this.bookService.applyDiscount(newDiscount);
      this.loadBooks();
      this.discountForm.reset({ type: 'book', amount: 0 });
    }
  }

  deleteDiscount(id: number): void {
    this.bookService.removeDiscount(this.discounts.find(discount => discount.id === id)!);
    this.discounts = this.discounts.filter(discount => discount.id !== id);
    this.saveDiscounts();
  }

  setActiveTab(tab: 'books' | 'reviews' | 'discounts'): void {
    this.activeTab = tab;
  }
}
