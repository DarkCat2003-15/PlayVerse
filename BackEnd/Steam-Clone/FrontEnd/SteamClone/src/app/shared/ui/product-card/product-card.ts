import { CurrencyPipe } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { Product } from '../../../Core/api/models/product.interface';

@Component({
  selector: 'app-product-card',
  standalone: true,
  imports: [CurrencyPipe, MatButtonModule, MatCardModule, MatChipsModule],
  templateUrl: './product-card.html',
  styleUrl: './product-card.scss',
})
export class ProductCard {
  @Input({ required: true }) product!: Product;
  @Output() detailRequested = new EventEmitter<Product>();

  showDetail(): void {
    this.detailRequested.emit(this.product);
  }
}
