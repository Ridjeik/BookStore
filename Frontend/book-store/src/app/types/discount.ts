export interface Discount {
  id: number;
  amount: number;
  type: 'Book' | 'Genre' | 'Author';
  value: string;
}
