import { CurrencyPipe, DatePipe } from '@angular/common';
import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { RouterLink } from '@angular/router';
import { Product } from '../../../../Core/api/models/product.interface';
import { CatalogService } from '../../../../Core/api/catalog.service';
import { artworkForTitle, gameCardToProduct } from '../../../../Core/api/game-mapper';
import { Game } from '../../../../Core/api/models/catalog.interface';
import { AuthService } from '../../../../Core/auth/auth.service';
import { ProductCard } from '../../../../shared/ui/product-card/product-card';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CurrencyPipe, DatePipe, MatButtonModule, MatCardModule, MatChipsModule, ProductCard, RouterLink],
  templateUrl: './home.html',
  styleUrl: './home.scss',
})
export class Home implements OnInit {
  private readonly catalogService = inject(CatalogService);
  readonly authService = inject(AuthService);
  private readonly fallbackGames: Product[] = [
    {
      id: 1,
      title: 'Elden Ring',
      description: 'Juego de accion y aventura en un mundo abierto.',
      price: 59.99,
      category: 'RPG',
      thumbnail:
        'https://shared.cloudflare.steamstatic.com/store_item_assets/steam/apps/1245620/header.jpg',
      rating: 'Muy positivo',
      tags: ['Accion', 'Aventura'],
    },
    {
      id: 2,
      title: 'Stardew Valley',
      description: 'Juego tranquilo de granja y simulacion.',
      price: 14.99,
      category: 'Indie',
      thumbnail:
        'https://shared.cloudflare.steamstatic.com/store_item_assets/steam/apps/413150/header.jpg',
      rating: 'Excelente',
      tags: ['Granja', 'Relax'],
    },
    {
      id: 3,
      title: 'Counter-Strike 2',
      description: 'Juego competitivo en equipos.',
      price: 0,
      category: 'FPS',
      thumbnail: 'https://shared.cloudflare.steamstatic.com/store_item_assets/steam/apps/730/header.jpg',
      rating: 'Popular',
      tags: ['Online', 'Competitivo'],
    },
  ];

  readonly featuredGames = signal<Product[]>(this.fallbackGames);
  readonly selectedProduct = signal<Product | null>(null);
  readonly selectedGame = signal<Game | null>(null);
  readonly detailLoading = signal(false);
  readonly detailError = signal('');

  readonly detailImage = computed(() => {
    const game = this.selectedGame();
    const product = this.selectedProduct();

    return game?.imageUrl?.trim() || product?.thumbnail || artworkForTitle(game?.title ?? product?.title ?? '');
  });

  readonly activeOffer = computed(() => {
    const offers = this.selectedGame()?.offers ?? [];
    return offers.find((offer) => offer.isActive) ?? offers[0] ?? null;
  });

  ngOnInit(): void {
    this.catalogService.getGames().subscribe({
      next: (games) => {
        this.featuredGames.set(
          games.length ? games.slice(0, 3).map(gameCardToProduct) : this.fallbackGames,
        );
      },
      error: () => {
        this.featuredGames.set(this.fallbackGames);
      },
    });
  }

  showDetail(product: Product): void {
    this.selectedProduct.set(product);
    this.selectedGame.set(null);
    this.detailError.set('');

    if (typeof product.id !== 'string') {
      return;
    }

    this.detailLoading.set(true);
    this.catalogService.getGame(product.id).subscribe({
      next: (game) => {
        this.selectedGame.set(game);
        this.detailLoading.set(false);
      },
      error: () => {
        this.detailError.set('No se pudo cargar el detalle completo desde la API.');
        this.detailLoading.set(false);
      },
    });
  }

  closeDetail(): void {
    this.selectedProduct.set(null);
    this.selectedGame.set(null);
    this.detailError.set('');
    this.detailLoading.set(false);
  }
}
