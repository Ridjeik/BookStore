import { Component } from '@angular/core';
import { Book } from '../../types/book';
import { OnInit } from '@angular/core';
import { BookService } from '../../services/book.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-catalog',
  standalone: true,
  imports: [
    FormsModule,
    CommonModule
  ],
  templateUrl: './catalog.component.html',
  styleUrl: './catalog.component.scss'
})
export class CatalogComponent implements OnInit {
  books: Book[] = [];
  filteredBooks: Book[] = [];
  displayedBooks: Book[] = [];
  searchTerm: string = '';
  sortOrder: string = 'asc';
  currentPage: number = 1;
  pageSize: number = 20;

  categories: Set<string> = new Set();
  authors: Set<string> = new Set();
  selectedCategories: Set<string> = new Set();
  selectedAuthors: Set<string> = new Set();
  priceRange: { min: number; max: number } = { min: 0, max: 100 };
  selectedPriceRange: { min: number; max: number } = { min: 0, max: 100 };

  constructor(private bookService: BookService,
    private router: Router
  ) {}

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
  }

  applyFilters(): void {

    this.filteredBooks = this.bookService
      .filterBooks(this.searchTerm)
      .filter(book =>
        (!this.selectedCategories.size || this.selectedCategories.has(book.category)) &&
        (!this.selectedAuthors.size || this.selectedAuthors.has(book.author)) &&
        book.price >= this.selectedPriceRange.min &&
        book.price <= this.selectedPriceRange.max
      );

    this.filteredBooks = this.bookService.sortBooks(this.filteredBooks, this.sortOrder);
    this.updateDisplayedBooks();
  }

  addToCart(event:Event, book: Book): void {

    event.stopPropagation();
  }

  cardOnClick(id: number) {
    console.log('cardOnClick', id);
    this.router.navigate(['/catalog', id]);
  }

  onSearch(): void {
    this.applyFilters();
  }

  onSort(): void {
    this.applyFilters();
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
}
