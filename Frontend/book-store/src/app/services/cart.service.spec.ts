import { TestBed } from '@angular/core/testing';
import { CartService } from './cart.service';
import { AuthService } from './auth.service';

describe('CartService', () => {
  let service: CartService;
  let authServiceMock: jasmine.SpyObj<AuthService>;

  beforeEach(() => {
    authServiceMock = jasmine.createSpyObj('AuthService', ['getUserInfo']);
    authServiceMock.getUserInfo.and.returnValue({ id: 'user1' });

    TestBed.configureTestingModule({
      providers: [
        CartService,
        { provide: AuthService, useValue: authServiceMock }
      ]
    });
    service = TestBed.inject(CartService);

    service.clearCart();
  });

  it('(smoke) should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should add an item to the cart', () => {
    service.addItem(1, 2);
    const cartItems = service.getCartItems();
    expect(cartItems).toEqual([{ itemId: 1, quantity: 2 }]);
  });

  it('should increase quantity when adding an existing item', () => {
    service.addItem(1, 2);
    service.addItem(1, 3);
    const cartItems = service.getCartItems();
    expect(cartItems).toEqual([{ itemId: 1, quantity: 5 }]);
  });

  it('should remove an item from the cart', () => {
    service.addItem(1, 2);
    service.addItem(2, 1);
    service.removeItem(1);
    const cartItems = service.getCartItems();
    expect(cartItems).toEqual([{ itemId: 2, quantity: 1 }]);
  });

  it('should clear the cart', () => {
    service.addItem(1, 2);
    service.addItem(2, 1);
    service.clearCart();
    const cartItems = service.getCartItems();
    expect(cartItems).toEqual([]);
  });
});

