import { TestBed } from '@angular/core/testing';

import { BookService } from './book.service';
import { ActivatedRoute } from '@angular/router';
import { ReviewService } from './review.service';
import { RouterTestingModule } from '@angular/router/testing';
import { MessageService } from 'primeng/api';

describe('BookService', () => {
  let service: BookService;
  let localStorageMock: { [key: string]: string };

  beforeEach(() => {
    localStorageMock = {};

    spyOn(localStorage, 'getItem').and.callFake((key: string) => localStorageMock[key] || null);
    spyOn(localStorage, 'setItem').and.callFake((key: string, value: string) => {
      localStorageMock[key] = value;
    });

    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      providers: [BookService, ReviewService, MessageService],
    });
    service = TestBed.inject(BookService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should retrieve books', () => {
    const books = service.getBooks();
    expect(books.length).toBeGreaterThan(0);
    expect(localStorage.setItem).toHaveBeenCalledWith(
      'books',
      jasmine.any(String)
    );
  });

  it('should retrieve books by id', () => {
    const mockBooks = JSON.stringify([
      { id: 1, title: 'Mock Book', author: 'Test Author' },
    ]);
    localStorageMock['books'] = mockBooks;

    const books = service.getBooks();
    expect(books.length).toBe(24);
    expect(books[0].title).toBe('All Systems Red');
  });

  it('should save books', () => {
    const newBook = {
      title: 'All Systems Red',
      author: 'Martha Wells',
      description: 'In a future where humans have created self-aware robots, a murderbot must navigate its existence and its interactions with humans while trying to understand its own feelings.',
      price: 15.99,
      coverImage: 'https://upload.wikimedia.org/wikipedia/en/8/8f/All_Systems_Red_-_The_Murderbot_Diaries_1_%28cover%29.jpg',
      category: 'Science Fiction',
      publisher: 'Tor.com',
      publicationDate: new Date('2017-05-02'),
      pageCount: 144,
      isbn: '9780765397539',
      hasDiscount: false,
      discount: 0,
      rating: 0,
      copiesAvailable: 3
    };

    service.addBook(newBook);

    const savedBooks = JSON.parse(localStorageMock['books']);
    expect(savedBooks.length).toBe(25);
    expect(savedBooks[24].title).toBe('All Systems Red');
  });
});
