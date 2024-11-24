export interface Review {
  id: string;
  bookId: number;
  userId: string;
  username: string;
  rating: number;
  text: string;
  date: string;
}