import { Component } from '@angular/core';
import { Book } from '../../types/book';
import { OnInit } from '@angular/core';
import { BookService } from '../../services/book.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { CartService } from '../../services/cart.service';
import { ToastModule} from 'primeng/toast';
import { ButtonModule } from 'primeng/button';
import { MessageService } from 'primeng/api';
import { RippleModule } from 'primeng/ripple';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-catalog',
  standalone: true,
  imports: [
    FormsModule,
    CommonModule,
    ToastModule,
    RippleModule
  ],
  templateUrl: './catalog.component.html',
  styleUrl: './catalog.component.scss'
})
export class CatalogComponent implements OnInit {
  books: Book[] = [];
  filteredBooks: Book[] = [];
  displayedBooks: Book[] = [];
  searchTerm: string = '';
  sortOrder: string = 'rating';
  currentPage: number = 1;
  pageSize: number = 20;

  categories: Set<string> = new Set();
  authors: Set<string> = new Set();
  selectedCategories: Set<string> = new Set();
  selectedAuthors: Set<string> = new Set();
  priceRange: { min: number; max: number } = { min: 0, max: 100 };
  selectedPriceRange: { min: number; max: number } = { min: 0, max: 100 };

  pageRange: { min: number; max: number } = { min: 0, max: 100 };
  selectedPageRange: { min: number; max: number } = { min: 0, max: 100 };

  constructor(private bookService: BookService,
    private messageService: MessageService,
    private cartService: CartService,
    private router: Router
  ) {}

  show() {
    this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Message Content' });
  }

  ngOnInit(): void {
    this.books = this.bookService.getBooks();
    this.extractFilterOptions();
    this.applyFilters();
  }

  extractFilterOptions(): void {
    this.books.forEach(book => {
      this.categories.add(book.category);
      this.authors.add(book.author);
    });

    const prices = this.books.map(book => book.price);
    this.priceRange = { min: Math.min(...prices), max: Math.max(...prices) };
    this.selectedPriceRange = { ...this.priceRange };

    const pages = this.books.map(book => book.pageCount);
    this.pageRange = { min: Math.min(...pages), max: Math.max(...pages) };
    this.selectedPageRange = { ...this.pageRange };

  }

  applyFilters(): void {

    this.filteredBooks = this.bookService.getBooks()
      .filter(book =>
        (book.title.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        book.author.toLowerCase().includes(this.searchTerm.toLowerCase())) &&
        (!this.selectedCategories.size || this.selectedCategories.has(book.category)) &&
        (!this.selectedAuthors.size || this.selectedAuthors.has(book.author)) &&
        book.price >= this.selectedPriceRange.min &&
        book.price <= this.selectedPriceRange.max &&
        book.pageCount >= this.selectedPageRange.min &&
        book.pageCount <= this.selectedPageRange.max
      );

      this.sortBooks();
      this.updateDisplayedBooks();
  }

  sortBooks(): void {
    this.filteredBooks.sort((a, b) => {
      switch (this.sortOrder) {
        case'rating':
          return b.rating - a.rating;
        case 'titleAsc':
          return a.title.localeCompare(b.title);
        case 'titleDesc':
          return b.title.localeCompare(a.title);
        case 'priceAsc':
          return a.price - b.price;
        case 'priceDesc':
          return b.price - a.price;
        default:
          return 0;
      }
    });
  }

  addToCart(event:Event, book: Book): void {

    event.stopPropagation();

    this.cartService.addItem(book.id);

    this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Book added to cart' });
  }

  cardOnClick(id: number) {
    console.log('cardOnClick', id);
    this.router.navigate(['/catalog', id]);
  }

  onSearch(): void {
    this.currentPage = 1;
    this.applyFilters();
  }

  onSort(): void {
    this.applyFilters();
  }

  clearSearch(): void {
    this.searchTerm = '';
    this.onSearch();
  }


  toggleCategory(category: string): void {
    this.selectedCategories.has(category) ? this.selectedCategories.delete(category) : this.selectedCategories.add(category);
    this.applyFilters();
  }

  toggleAuthor(author: string): void {
    this.selectedAuthors.has(author) ? this.selectedAuthors.delete(author) : this.selectedAuthors.add(author);
    this.applyFilters();
  }

  updateDisplayedBooks(): void {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.displayedBooks = this.filteredBooks.slice(startIndex, endIndex);
  }

  goToPage(page: number): void {
    this.currentPage = page;
    this.updateDisplayedBooks();
  }

  updatePriceRange(): void {
    this.applyFilters();
  }

  updatePageRange(): void {
    this.applyFilters();
  }

  getRatingStars(rating: number): number[] {
    return Array(5).fill(0).map((_, index) => index < rating ? 1 : 0);
  }
}
