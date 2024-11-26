import { TestBed } from '@angular/core/testing';
import { MessageService } from 'primeng/api';
import { AuthService } from '../services/auth.service';
import { adminGuard } from './admin.guard';

describe('adminGuard', () => {
  let authServiceMock: jasmine.SpyObj<AuthService>;
  let messageServiceMock: jasmine.SpyObj<MessageService>;

  beforeEach(() => {
    authServiceMock = jasmine.createSpyObj('AuthService', ['isLoggedIn', 'getUserInfo']);
    messageServiceMock = jasmine.createSpyObj('MessageService', ['add']);

    TestBed.configureTestingModule({
      providers: [
        { provide: AuthService, useValue: authServiceMock },
        { provide: MessageService, useValue: messageServiceMock }
      ]
    });
  });

  it('should allow access for logged-in admin users', () => {
    authServiceMock.isLoggedIn.and.returnValue(true);
    authServiceMock.getUserInfo.and.returnValue({ role: 'admin' });

    const result = TestBed.runInInjectionContext(() => adminGuard({} as any, {} as any));

    expect(result).toBe(true);
  });

  it('should deny access for logged-in non-admin users', () => {
    authServiceMock.isLoggedIn.and.returnValue(true);
    authServiceMock.getUserInfo.and.returnValue({ role: 'user' });

    const result = TestBed.runInInjectionContext(() => adminGuard({} as any, {} as any));

    expect(result).toBe(false);
    expect(messageServiceMock.add).toHaveBeenCalledWith({
      severity: 'error',
      summary: 'Error',
      detail: 'You need to be an admin to access this page'
    });
  });

  it('should deny access for non-logged-in users', () => {
    authServiceMock.isLoggedIn.and.returnValue(false);

    const result = TestBed.runInInjectionContext(() => adminGuard({} as any, {} as any));

    expect(result).toBe(false);
    expect(messageServiceMock.add).toHaveBeenCalledWith({
      severity: 'info',
      summary: 'Info',
      detail: 'You need to login to access this page'
    });
  });
});
