export interface Book {
  id: number;
  title: string;
  author: string;
  publisher: string;
  publicationDate: Date;
  pageCount: number;
  isbn: string;
  description: string;
  price: number;
  coverImage: string;
  category: string;
  hasDiscount: boolean;
  discount: number;
  rating: number;
}
