import { Injectable } from '@angular/core';
import { Book } from '../types/book';
import { of } from 'rxjs';
import { Discount } from '../types/discount';
import { ReviewService } from './review.service';

@Injectable({
  providedIn: 'root'
})
export class BookService {

  private readonly STORAGE_KEY = 'books';

  private defaultBooks: Book[] = [
    {
      id: 1,
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
      rating: 0
    },
    {
      id: 2,
      title: 'Network Effect',
      author: 'Martha Wells',
      description: 'The first full-length novel in the Murderbot Diaries series. It follows Murderbot as it must deal with new threats to its friends while uncovering deeper truths about itself and its place in the universe.',
      price: 18.99,
      coverImage: 'https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1640597293i/52381770.jpg',
      category: 'Science Fiction',
      publisher: 'Tor.com',
      publicationDate: new Date('2020-05-05'),
      pageCount: 352,
      isbn: '9781250229861',
      hasDiscount: false,
      discount: 0,
      rating: 0
    },
    {
      id: 3,
      title: 'Witch King',
      author: 'Martha Wells',
      description: 'In this fantasy novella, a demon must contend with a scheming sorcerer and a powerful king in a tale that blends politics, magic, and complex character relationships.',
      price: 12.99,
      coverImage: 'https://i.gr-assets.com/images/S/compressed.photo.goodreads.com/books/1663769801l/61885085.jpg',
      category: 'Fantasy',
      publisher: 'Tor Books',
      publicationDate: new Date('2023-05-30'),
      pageCount: 432,
      isbn: '9781250826794',
      hasDiscount: false,
      discount: 0,
      rating: 0
    },
    {
      id: 4,
      title: 'Dune',
      author: 'Frank Herbert',
      description: 'A science fiction classic that explores themes of power, religion, and humanity as young Paul Atreides navigates a desert planet and its politics.',
      price: 16.99,
      coverImage: 'https://static.yakaboo.ua/media/cloudflare/product/webp/600x840/i/m/img243_1_21.jpg',
      category: 'Science Fiction',
      publisher: 'Chilton Books',
      publicationDate: new Date('1965-08-01'),
      pageCount: 412,
      isbn: '9780441013593',
      hasDiscount: false,
      discount: 0,
      rating: 0
    },
    {
      id: 5,
      title: 'To Kill a Mockingbird',
      author: 'Harper Lee',
      description: 'A coming-of-age story set in the American South, dealing with serious themes of racial injustice and moral growth.',
      price: 10.99,
      coverImage: 'https://cdn.britannica.com/21/182021-050-666DB6B1/book-cover-To-Kill-a-Mockingbird-many-1961.jpg',
      category: 'Classic Literature',
      publisher: 'J.B. Lippincott & Co.',
      publicationDate: new Date('1960-07-11'),
      pageCount: 281,
      isbn: '9780061120084',
      hasDiscount: false,
      discount: 0,
      rating: 0
    },
    {
      id: 6,
      title: '1984',
      author: 'George Orwell',
      description: 'A dystopian novel that warns of a future under totalitarian rule, where privacy and individuality are suppressed.',
      price: 9.99,
      coverImage: 'https://m.media-amazon.com/images/I/61NAx5pd6XL._AC_UF1000,1000_QL80_.jpg',
      category: 'Dystopian Fiction',
      publisher: 'Secker & Warburg',
      publicationDate: new Date('1949-06-08'),
      pageCount: 328,
      isbn: '9780451524935',
      hasDiscount: false,
      discount: 0,
      rating: 0
    },
    {
      id: 7,
      title: 'The Hobbit',
      author: 'J.R.R. Tolkien',
      description: 'A fantasy adventure where Bilbo Baggins, a hobbit, is reluctantly pulled into a quest to recover treasure guarded by a dragon.',
      price: 14.99,
      coverImage: 'https://upload.wikimedia.org/wikipedia/en/3/30/Hobbit_cover.JPG',
      category: 'Fantasy',
      publisher: 'George Allen & Unwin',
      publicationDate: new Date('1937-09-21'),
      pageCount: 310,
      isbn: '9780547928227',
      hasDiscount: false,
      discount: 0,
      rating: 0
    },
    {
      id: 8,
      title: 'Pride and Prejudice',
      author: 'Jane Austen',
      description: 'A classic romance novel that follows Elizabeth Bennet as she navigates social norms, family expectations, and love.',
      price: 8.99,
      coverImage: 'https://m.media-amazon.com/images/I/712P0p5cXIL._AC_UF1000,1000_QL80_.jpg',
      category: 'Classic Literature',
      publisher: 'T. Egerton',
      publicationDate: new Date('1813-01-28'),
      pageCount: 279,
      isbn: '9781503290563',
      hasDiscount: false,
      discount: 0,
      rating: 0
    },
    {
      id: 9,
      title: 'The Catcher in the Rye',
      author: 'J.D. Salinger',
      description: 'A novel about teenage angst and alienation, following the story of Holden Caulfield as he rebels against societal expectations.',
      price: 11.99,
      coverImage: 'https://static.yakaboo.ua/media/cloudflare/product/webp/600x840/3/0/306871_66985190.jpg',
      category: 'Classic Literature',
      publisher: 'Little, Brown and Company',
      publicationDate: new Date('1951-07-16'),
      pageCount: 277,
      isbn: '9780316769488',
      hasDiscount: false,
      discount: 0,
      rating: 0
    },
    {
      id: 10,
      title: 'The Shining',
      author: 'Stephen King',
      description: 'A horror novel about a family staying in a haunted hotel, where supernatural events and personal struggles lead to terrifying consequences.',
      price: 13.99,
      coverImage: 'https://m.media-amazon.com/images/I/91U7HNa2NQL.jpg',
      category: 'Horror',
      publisher: 'Doubleday',
      publicationDate: new Date('1977-01-28'),
      pageCount: 447,
      isbn: '9780307743657',
      hasDiscount: false,
      discount: 0,
      rating: 0
    },
    {
      id: 11,
      title: 'The Subtle Art of Not Giving a F*ck',
      author: 'Mark Manson',
      description: 'A self-help book that emphasizes embracing life\'s challenges and letting go of unrealistic expectations.',
      price: 15.99,
      coverImage: 'https://m.media-amazon.com/images/I/71QKQ9mwV7L._AC_UF894,1000_QL80_.jpg',
      category: 'Self-Help',
      publisher: 'HarperOne',
      publicationDate: new Date('2016-09-13'),
      pageCount: 224,
      isbn: '9780062457714',
      hasDiscount: false,
      discount: 0,
      rating: 0
    },
    {
      id: 12,
      title: 'Becoming',
      author: 'Michelle Obama',
      description: 'The memoir of former First Lady Michelle Obama, exploring her personal journey and experiences in the White House.',
      price: 17.99,
      coverImage: 'https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1528206996i/38746485.jpg',
      category: 'Memoir',
      publisher: 'Crown Publishing Group',
      publicationDate: new Date('2018-11-13'),
      pageCount: 448,
      isbn: '9781524763138',
      hasDiscount: false,
      discount: 0,
      rating: 0
    },
    {
      id: 13,
      title: 'The Road',
      author: 'Cormac McCarthy',
      description: 'A post-apocalyptic novel following a father and son as they navigate a barren world, facing moral and physical challenges.',
      price: 13.99,
      coverImage: 'https://upload.wikimedia.org/wikipedia/commons/2/27/The-road.jpg',
      category: 'Dystopian Fiction',
      publisher: 'Alfred A. Knopf',
      publicationDate: new Date('2006-09-26'),
      pageCount: 287,
      isbn: '9780307387899',
      hasDiscount: false,
      discount: 0,
      rating: 0
    },
    {
      id: 14,
      title: 'Educated',
      author: 'Tara Westover',
      description: 'A memoir about a young woman who grows up in a strict and abusive household in rural Idaho and eventually escapes to pursue an education.',
      price: 14.99,
      coverImage: 'https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1506026635i/35133922.jpg',
      category: 'Memoir',
      publisher: 'Random House',
      publicationDate: new Date('2018-02-20'),
      pageCount: 400,
      isbn: '9780399590504',
      hasDiscount: false,
      discount: 0,
      rating: 0
    },
    {
      id: 15,
      title: 'The Girl on the Train',
      author: 'Paula Hawkins',
      description: 'A psychological thriller revolving around the lives of three women and the secrets they hide.',
      price: 12.99,
      coverImage: 'https://m.media-amazon.com/images/M/MV5BZTg4MGViYTMtYTFkMC00YzVlLWExZDItOTM4NTUwMjc2NTE2XkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg',
      category: 'Thriller',
      publisher: 'Riverhead Books',
      publicationDate: new Date('2015-01-13'),
      pageCount: 395,
      isbn: '9781594633669',
      hasDiscount: false,
      discount: 0,
      rating: 0
    },
    {
      id: 16,
      title: 'Sapiens: A Brief History of Humankind',
      author: 'Yuval Noah Harari',
      description: 'A historical analysis of humanity, exploring the development of human societies, cultures, and technologies.',
      price: 19.99,
      coverImage: 'https://static.yakaboo.ua/media/cloudflare/product/webp/600x840/7/1/71ukqqzd-vl.jpg',
      category: 'Non-fiction',
      publisher: 'Harvill Secker',
      publicationDate: new Date('2011-01-01'),
      pageCount: 443,
      isbn: '9780062316097',
      hasDiscount: false,
      discount: 0,
      rating: 0
    },
    {
      id: 17,
      title: 'The Night Circus',
      author: 'Erin Morgenstern',
      description: 'A fantasy novel set around a mysterious, magical circus that opens without warning and only operates at night.',
      price: 16.99,
      coverImage: 'https://static.yakaboo.ua/media/catalog/product/9/7/9781784871055.jpg',
      category: 'Fantasy',
      publisher: 'Doubleday',
      publicationDate: new Date('2011-09-13'),
      pageCount: 387,
      isbn: '9780385534635',
      hasDiscount: false,
      discount: 0,
      rating: 0
    },
    {
      id: 18,
      title: 'The Silent Patient',
      author: 'Alex Michaelides',
      description: 'A psychological thriller about a woman who shoots her husband and then stops speaking, and the therapist who tries to uncover the truth.',
      price: 14.99,
      coverImage: 'https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1668782119i/40097951.jpg',
      category: 'Mystery',
      publisher: 'Celadon Books',
      publicationDate: new Date('2019-02-05'),
      pageCount: 325,
      isbn: '9781250301697',
      hasDiscount: false,
      discount: 0,
      rating: 0
    },
    {
      id: 19,
      title: 'Circe',
      author: 'Madeline Miller',
      description: 'A retelling of the Greek myth of Circe, the witch who transforms Odysseus’s men into swine, and her journey to self-discovery.',
      price: 15.99,
      coverImage: 'https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1565909496i/35959740.jpg',
      category: 'Fantasy',
      publisher: 'Little, Brown and Company',
      publicationDate: new Date('2018-04-10'),
      pageCount: 393,
      isbn: '9780316556347',
      hasDiscount: false,
      discount: 0,
      rating: 0
    },
    {
      id: 20,
      title: 'Where the Crawdads Sing',
      author: 'Delia Owens',
      description: 'A mystery set in the marshes of North Carolina, following a young girl as she grows up isolated and becomes the prime suspect in a murder investigation.',
      price: 16.99,
      coverImage: 'https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1582135294i/36809135.jpg',
      category: 'Mystery',
      publisher: 'G.P. Putnam\'s Sons',
      publicationDate: new Date('2018-08-14'),
      pageCount: 384,
      isbn: '9780735219090',
      hasDiscount: false,
      discount: 0,
      rating: 0
    },
    {
      id: 21,
      title: 'The Invisible Man',
      author: 'H.G. Wells',
      description: 'A science fiction novel about a scientist who discovers the secret to invisibility and the disastrous consequences of his experiments.',
      price: 7.99,
      coverImage: 'https://m.media-amazon.com/images/I/51CWa1RwqIL._AC_UF1000,1000_QL80_.jpg',
      category: 'Science Fiction',
      publisher: 'Garland Publishing',
      publicationDate: new Date('1897-03-01'),
      pageCount: 210,
      isbn: '9780486275437',
      hasDiscount: false,
      discount: 0,
      rating: 0
    },
    {
      id: 22,
      title: 'Frankenstein',
      author: 'Mary Shelley',
      description: 'A Gothic novel about a scientist who creates a creature and the resulting tragic consequences.',
      price: 9.99,
      coverImage: 'https://www.britishbook.ua/upload/resize_cache/iblock/09a/9h8b9wcm4i4jzdj0s5ihqx1zyts1k5ux/1900_800_174b5ed2089e1946312e2a80dcd26f146/knyga_frankenstein.jpg',
      category: 'Horror',
      publisher: 'Lackington, Hughes, Harding, Mavor & Jones',
      publicationDate: new Date('1818-01-01'),
      pageCount: 280,
      isbn: '9780486282114',
      hasDiscount: false,
      discount: 0,
      rating: 0
    },
    {
      id: 23,
      title: 'Dracula',
      author: 'Bram Stoker',
      description: 'A Gothic horror novel about the infamous vampire Count Dracula, told through a series of letters and diary entries.',
      price: 12.99,
      coverImage: 'https://www.britishbook.ua/upload/resize_cache/iblock/dc5/q7gtnyn348s8y774qhvl9wcbctycu1m3/283_450_174b5ed2089e1946312e2a80dcd26f146/kniga_dracula.jpeg',
      category: 'Horror',
      publisher: 'Archibald Constable and Company',
      publicationDate: new Date('1897-05-26'),
      pageCount: 416,
      isbn: '9780486411093',
      hasDiscount: false,
      discount: 0,
      rating: 0
    },
    {
      id: 24,
      title: 'The Metamorphosis',
      author: 'Franz Kafka',
      description: 'A surreal novella about a man who wakes up one morning to find himself transformed into a giant insect, leading to a tragic exploration of alienation.',
      price: 6.99,
      coverImage: 'https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1646444605i/485894.jpg',
      category: 'Literary Fiction',
      publisher: 'Schocken Books',
      publicationDate: new Date('1915-01-01'),
      pageCount: 90,
      isbn: '9780805209991',
      hasDiscount: false,
      discount: 0,
      rating: 0
    }
  ];
  
  private books: Book[] = [];


  constructor(private reviewService: ReviewService) {
    this.loadBooks();
  }

  private loadBooks(): void {
    const storedBooks = localStorage.getItem(this.STORAGE_KEY);
    if (storedBooks) {
      this.books = JSON.parse(storedBooks);
    } else {
      this.books = this.defaultBooks;
      this.saveBooks();
    }
  }

  public saveBooks(): void {
    this.books = this.books.map(book => {return {...book, rating: this.reviewService.getAverageRating(book.id)}});
    localStorage.setItem(this.STORAGE_KEY, JSON.stringify(this.books));
  }

  getBooks(): Book[] {
    this.saveBooks();
    return this.books;
  }

  getBookById(id: number): Book | undefined {
    return this.books.find(book => book.id === id);
  }

  addBook(book: Omit<Book, 'id'>): Book {
    const newBook: Book = {
      ...book,
      id: this.generateUniqueId()
    };
    this.books.push(newBook);
    this.saveBooks();
    return newBook;
  }

  updateBook(updatedBook: Book): void {
    const index = this.books.findIndex(book => book.id === updatedBook.id);
    if (index !== -1) {
      this.books[index] = updatedBook;
      this.saveBooks();
    }
  }

  deleteBook(id: number): void {
    this.books = this.books.filter(book => book.id !== id);
    this.saveBooks();
  }

  applyDiscount(discount: Discount): void {

    this.books = this.books.map(book => {
      if (
        (discount.type === 'Book' && book.id === parseInt(discount.value)) ||
        (discount.type === 'Genre' && book.category === discount.value) ||
        (discount.type === 'Author' && book.author === discount.value)
      ) {
        return {
          ...book,
          hasDiscount: true,
          discount: discount.amount
        };
      }
      return book;
    });
    this.saveBooks();
  }

  removeDiscount(discount: Discount): void {
    this.books = this.books.map(book => {
      if (
        (discount.type === 'Book' && book.id === parseInt(discount.value)) ||
        (discount.type === 'Genre' && book.category === discount.value) ||
        (discount.type === 'Author' && book.author === discount.value)
      ) {
        return {
          ...book,
          hasDiscount: false,
          discount: 0
        };
      }
      return book;
    });
    this.saveBooks();
  }

  private generateUniqueId(): number {
    return Math.max(...this.books.map(book => book.id), 0) + 1;
  }

  getBooksByIds(ids: number[]):Book[] {
    const books = this.books.filter(book => ids.includes(book.id));
    return (books);
  }
}
