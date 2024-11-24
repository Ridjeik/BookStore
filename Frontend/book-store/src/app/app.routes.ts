import { Routes } from '@angular/router';
import { CatalogComponent } from './features/catalog/catalog.component';
import { BookDetailsComponent } from './features/book-details/book-details.component';
import { CartComponent } from './features/cart/cart.component';
import { userGuard } from './guards/user.guard';
import { AdminBooksComponent } from './features/admin-books/admin-books.component';

export const routes: Routes = [
  { path: 'catalog', component: CatalogComponent },
  { path: 'catalog/:id', component: BookDetailsComponent },
  { path: 'cart', component: CartComponent, canActivate: [userGuard] },
  { path: 'admin-books', component: AdminBooksComponent, canActivate: [userGuard] },
];
