import { Routes } from '@angular/router';
import { CatalogComponent } from './features/catalog/catalog.component';
import { BookDetailsComponent } from './features/book-details/book-details.component';

export const routes: Routes = [
  { path: 'catalog', component: CatalogComponent },
  { path: 'catalog/:id', component: BookDetailsComponent }
];
