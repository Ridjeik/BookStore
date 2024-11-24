import { Injectable } from '@angular/core';
import { Review } from '../types/review';
import { BookService } from './book.service';

@Injectable({
  providedIn: 'root'
})
export class ReviewService {
  private readonly STORAGE_KEY = 'book_reviews';

  getAllReviews(): Review[] {
    const reviews = localStorage.getItem(this.STORAGE_KEY);
    return reviews ? JSON.parse(reviews) : [];
  }

  getAverageRating(bookId: number): number {
    if (!this.getReviews(bookId).length) return 0;

    return this.getReviews(bookId)
      .reduce((acc: number, review: Review) => acc + review.rating, 0) / this.getReviews(bookId).length;
  }

  getReviews(bookId: number): Review[] {
    return this.getAllReviews()
      .filter((review: Review) => review.bookId === bookId)
      .sort((a: Review, b: Review) => 
        new Date(b.date).getTime() - new Date(a.date).getTime()
      );
  }

  addReview(review: Omit<Review, 'id' | 'date'>): Review {
    const reviews = this.getAllReviews();
    
    const newReview: Review = {
      ...review,
      id: crypto.randomUUID(),
      date: new Date().toISOString()
    };

    localStorage.setItem(
      this.STORAGE_KEY, 
      JSON.stringify([...reviews, newReview])
    );

    return newReview;
  }

  deleteReview(id: string): void {
    const reviews = this.getAllReviews();
    const updatedReviews = reviews.filter(review => review.id !== id);
    localStorage.setItem(this.STORAGE_KEY, JSON.stringify(updatedReviews));
  }
}
