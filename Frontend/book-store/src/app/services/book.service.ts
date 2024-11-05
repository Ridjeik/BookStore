import { Injectable } from '@angular/core';
import { Book } from '../types/book';

@Injectable({
  providedIn: 'root'
})
export class BookService {

  private books: Book[] = [
    {
      id: 1,
      title: 'All Systems Red',
      author: 'Martha Wells',
      description: 'In a future where humans have created self-aware robots, a murderbot must navigate its existence and its interactions with humans while trying to understand its own feelings.',
      price: 15.99,
      coverImage: 'https://upload.wikimedia.org/wikipedia/en/8/8f/All_Systems_Red_-_The_Murderbot_Diaries_1_%28cover%29.jpg',
      category: 'Science Fiction'
    },
    {
      id: 2,
      title: 'Network Effect',
      author: 'Martha Wells',
      description: 'The first full-length novel in the Murderbot Diaries series. It follows Murderbot as it must deal with new threats to its friends while uncovering deeper truths about itself and its place in the universe.',
      price: 18.99,
      coverImage: 'https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1640597293i/52381770.jpg',
      category: 'Science Fiction'
    },
    {
      id: 3,
      title: 'Witch King',
      author: 'Martha Wells',
      description: 'In this fantasy novella, a demon must contend with a scheming sorcerer and a powerful king in a tale that blends politics, magic, and complex character relationships.',
      price: 12.99,
      coverImage: 'https://i.gr-assets.com/images/S/compressed.photo.goodreads.com/books/1663769801l/61885085.jpg',
      category: 'Fantasy'
    },
    {
      id: 4,
      title: 'Dune',
      author: 'Frank Herbert',
      description: 'A science fiction classic that explores themes of power, religion, and humanity as young Paul Atreides navigates a desert planet and its politics.',
      price: 16.99,
      coverImage: 'https://static.yakaboo.ua/media/cloudflare/product/webp/600x840/i/m/img243_1_21.jpg',
      category: 'Science Fiction'
  },
  {
      id: 5,
      title: 'To Kill a Mockingbird',
      author: 'Harper Lee',
      description: 'A coming-of-age story set in the American South, dealing with serious themes of racial injustice and moral growth.',
      price: 10.99,
      coverImage: 'https://cdn.britannica.com/21/182021-050-666DB6B1/book-cover-To-Kill-a-Mockingbird-many-1961.jpg',
      category: 'Classic Literature'
  },
  {
      id: 6,
      title: '1984',
      author: 'George Orwell',
      description: 'A dystopian novel that warns of a future under totalitarian rule, where privacy and individuality are suppressed.',
      price: 9.99,
      coverImage: 'https://m.media-amazon.com/images/I/61NAx5pd6XL._AC_UF1000,1000_QL80_.jpg',
      category: 'Dystopian Fiction'
  },
  {
      id: 7,
      title: 'The Hobbit',
      author: 'J.R.R. Tolkien',
      description: 'A fantasy adventure where Bilbo Baggins, a hobbit, is reluctantly pulled into a quest to recover treasure guarded by a dragon.',
      price: 14.99,
      coverImage: 'https://upload.wikimedia.org/wikipedia/en/3/30/Hobbit_cover.JPG',
      category: 'Fantasy'
  },
  {
      id: 8,
      title: 'Pride and Prejudice',
      author: 'Jane Austen',
      description: 'A classic romance novel that follows Elizabeth Bennet as she navigates social norms, family expectations, and love.',
      price: 8.99,
      coverImage: 'https://m.media-amazon.com/images/I/712P0p5cXIL._AC_UF1000,1000_QL80_.jpg',
      category: 'Classic Literature'
  },
  {
      id: 9,
      title: 'The Catcher in the Rye',
      author: 'J.D. Salinger',
      description: 'A novel about teenage angst and alienation, following the story of Holden Caulfield as he rebels against societal expectations.',
      price: 11.99,
      coverImage: 'https://static.yakaboo.ua/media/cloudflare/product/webp/600x840/3/0/306871_66985190.jpg',
      category: 'Classic Literature'
  },
  {
      id: 10,
      title: 'The Shining',
      author: 'Stephen King',
      description: 'A horror novel about a family staying in a haunted hotel, where supernatural events and personal struggles lead to terrifying consequences.',
      price: 13.99,
      coverImage: 'https://m.media-amazon.com/images/I/91U7HNa2NQL.jpg',
      category: 'Horror'
  },
  {
      id: 11,
      title: 'The Subtle Art of Not Giving a F*ck',
      author: 'Mark Manson',
      description: 'A self-help book that emphasizes embracing life\'s challenges and letting go of unrealistic expectations.',
      price: 15.99,
      coverImage: 'https://m.media-amazon.com/images/I/71QKQ9mwV7L._AC_UF894,1000_QL80_.jpg',
      category: 'Self-Help'
  },
  {
      id: 12,
      title: 'Becoming',
      author: 'Michelle Obama',
      description: 'The memoir of former First Lady Michelle Obama, exploring her personal journey and experiences in the White House.',
      price: 17.99,
      coverImage: 'https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1528206996i/38746485.jpg',
      category: 'Memoir'
  },
  {
      id: 13,
      title: 'The Road',
      author: 'Cormac McCarthy',
      description: 'A post-apocalyptic novel following a father and son as they navigate a barren world, facing moral and physical challenges.',
      price: 13.99,
      coverImage: 'https://upload.wikimedia.org/wikipedia/commons/2/27/The-road.jpg',
      category: 'Dystopian Fiction'
  },
  {
      id: 14,
      title: 'Educated',
      author: 'Tara Westover',
      description: 'A memoir about a young woman who grows up in a strict and abusive household in rural Idaho and eventually escapes to pursue an education.',
      price: 14.99,
      coverImage: 'https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1506026635i/35133922.jpg',
      category: 'Memoir'
  },
  {
      id: 15,
      title: 'A Brief History of Time',
      author: 'Stephen Hawking',
      description: 'A groundbreaking exploration of the universe, time, and the nature of black holes, written for general readers.',
      price: 18.99,
      coverImage: 'https://m.media-amazon.com/images/I/91ebghaV-eL._AC_UF1000,1000_QL80_.jpg',
      category: 'Science'
  },
  {
      id: 16,
      title: 'Sapiens: A Brief History of Humankind',
      author: 'Yuval Noah Harari',
      description: 'An exploration of humanity\'s history, from early human species to the modern era, examining cultural, biological, and historical developments.',
      price: 20.99,
      coverImage: 'https://static.yakaboo.ua/media/cloudflare/product/webp/600x840/7/1/71ukqqzd-vl.jpg',
      category: 'History'
  },
  {
      id: 17,
      title: 'The Silent Patient',
      author: 'Alex Michaelides',
      description: 'A psychological thriller about a woman who is accused of murdering her husband and stops speaking, prompting a therapist to uncover the truth.',
      price: 13.99,
      coverImage: 'https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1668782119i/40097951.jpg',
      category: 'Thriller'
  },
  {
      id: 18,
      title: 'The Girl with the Dragon Tattoo',
      author: 'Stieg Larsson',
      description: 'A mystery novel following journalist Mikael Blomkvist and hacker Lisbeth Salander as they investigate a decades-old disappearance.',
      price: 12.99,
      coverImage: 'https://m.media-amazon.com/images/I/81ErH6RdLpL._UF1000,1000_QL80_.jpg',
      category: 'Mystery'
  },
  {
      id: 19,
      title: 'Atomic Habits',
      author: 'James Clear',
      description: 'A guide on building good habits and breaking bad ones through small, incremental changes.',
      price: 19.99,
      coverImage: 'https://static.yakaboo.ua/media/catalog/product/9/7/9781847941831.jpg',
      category: 'Self-Help'
  },
  {
      id: 20,
      title: 'The Goldfinch',
      author: 'Donna Tartt',
      description: 'A Pulitzer Prize-winning novel about a boy whose life changes after a tragedy, leading him into a world of art and intrigue.',
      price: 14.99,
      coverImage: 'https://m.media-amazon.com/images/I/81QxwwBNU9L._AC_UF1000,1000_QL80_.jpg',
      category: 'Contemporary Fiction'
  },
  {
      id: 21,
      title: 'The Seven Husbands of Evelyn Hugo',
      author: 'Taylor Jenkins Reid',
      description: 'A historical fiction novel following an aging Hollywood star as she recounts her tumultuous love life and career.',
      price: 12.99,
      coverImage: 'https://static.yakaboo.ua/media/cloudflare/product/webp/600x840/7/1/71pievu3eel.jpg',
      category: 'Historical Fiction'
  },
  {
      id: 22,
      title: 'The Midnight Library',
      author: 'Matt Haig',
      description: 'A thought-provoking novel about a woman who, in the afterlife, explores various lives she could have lived.',
      price: 15.99,
      coverImage: 'https://www.britishbook.ua/upload/resize_cache/iblock/5a3/0hpvvpvp102uupqvem4gu1kluql5mwdl/291_448_174b5ed2089e1946312e2a80dcd26f146/kniga_the_midnight_library.jpg',
      category: 'Fantasy'
  },
  {
      id: 23,
      title: 'Good Omens',
      author: 'Neil Gaiman and Terry Pratchett',
      description: 'A comedic fantasy about an angel and a demon who team up to prevent the apocalypse.',
      price: 11.99,
      coverImage: 'https://i.gr-assets.com/images/S/compressed.photo.goodreads.com/books/1615552073l/12067.jpg',
      category: 'Fantasy'
  },
  {
      id: 24,
      title: 'A Man Called Ove',
      author: 'Fredrik Backman',
      description: 'A heartwarming novel about a grumpy old man whose life is changed by his new, lively neighbors.',
      price: 13.99,
      coverImage: 'https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1405259930i/18774964.jpg',
      category: 'Contemporary Fiction'
  }
  ];

  constructor() {}

  getBooks(): Book[] {
    return this.books;
  }

  getBookById(id: number): Book | undefined { 
    return this.books.find(book => book.id === id);
  }

  filterBooks(searchTerm: string): Book[] {
    const lowerSearch = searchTerm.toLowerCase();
    return this.books.filter(book => 
      book.title.toLowerCase().includes(lowerSearch) || 
      book.author.toLowerCase().includes(lowerSearch)
    );
  }

  sortBooks(books: Book[], sortOrder: string): Book[] {
    return books.slice().sort((a, b) => 
      sortOrder === 'asc' ? a.price - b.price : b.price - a.price
    );
  }
}
