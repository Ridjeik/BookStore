import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { BookService } from '../../services/book.service';
import { Book } from '../../types/book';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-book-details',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink
  ],
  templateUrl: './book-details.component.html',
  styleUrl: './book-details.component.scss'
})
export class BookDetailsComponent implements OnInit {
  constructor(private route: ActivatedRoute,
              private bookService: BookService
  ) {}

  currentBook: Book | undefined;

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    if (id) {
      this.currentBook = this.bookService.getBookById(parseInt(id));
    }
  }

  addToCart(book: Book): void {

  }

}
