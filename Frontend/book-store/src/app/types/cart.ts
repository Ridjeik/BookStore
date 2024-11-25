export interface Cart {
  userId: number;
  items: {
    itemId: number;
    quantity: number;
  }[];
}
