import { TestBed } from '@angular/core/testing';
import { ReviewService } from './review.service';
import { Review } from '../types/review';

describe('ReviewService', () => {
  let service: ReviewService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ReviewService]
    });
    service = TestBed.inject(ReviewService);
    
    service.getAllReviews().forEach((review: Review) => {
      service.deleteReview(review.id);
    });
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should add a review', () => {
    const newReview = service.addReview({
      bookId: 1,
      userId: 'user1',
      username: 'user1',
      rating: 4,
      text: 'Great book!'
    });

    expect(newReview.id).toBeTruthy();
    expect(newReview.date).toBeTruthy();
    expect(service.getReviews(1).length).toBe(1);
  });

  it('should get reviews for a specific book', () => {
    service.addReview({ bookId: 1, userId: 'user1',username: 'user1', rating: 4, text: 'Great book!' });
    service.addReview({ bookId: 1, userId: 'user2', username: 'user2',rating: 5, text: 'Awesome!' });
    service.addReview({ bookId: 2, userId: 'user3',username: 'user3', rating: 3, text: 'Okay' });

    const bookOneReviews = service.getReviews(1);
    expect(bookOneReviews.length).toBe(2);
    expect(bookOneReviews[0].rating).toBe(4);
  });

  it('should calculate average rating correctly', () => {
    service.addReview({ bookId: 1, userId: 'user1', username: 'user1', rating: 4, text: 'Great' });
    service.addReview({ bookId: 1, userId: 'user2', username: 'user2', rating: 5, text: 'Awesome' });
    service.addReview({ bookId: 1, userId: 'user3', username: 'user3', rating: 3, text: 'Good' });

    expect(service.getAverageRating(1)).toBe(4);
  });

  it('should delete a review', () => {
    const review = service.addReview({ bookId: 1, userId: 'user1', username: 'user1', rating: 4, text: 'Great' });
    expect(service.getReviews(1).length).toBe(1);

    service.deleteReview(review.id);
    expect(service.getReviews(1).length).toBe(0);
  });

  it('should return 0 for average rating when no reviews exist', () => {
    expect(service.getAverageRating(1)).toBe(0);
  });
});

