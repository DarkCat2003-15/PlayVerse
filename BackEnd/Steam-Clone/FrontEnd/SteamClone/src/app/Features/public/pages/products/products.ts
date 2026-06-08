import { CurrencyPipe, DatePipe } from '@angular/common';
import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { Product } from '../../../../Core/api/models/product.interface';
import { CatalogService } from '../../../../Core/api/catalog.service';
import { artworkForTitle, gameCardToProduct } from '../../../../Core/api/game-mapper';
import { Game } from '../../../../Core/api/models/catalog.interface';
import { LibraryItem, WishlistItem } from '../../../../Core/api/models/community.interface';
import { StoreCommunityService } from '../../../../Core/api/store-community.service';
import { AuthService } from '../../../../Core/auth/auth.service';
import { ProductCard } from '../../../../shared/ui/product-card/product-card';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [
    CurrencyPipe,
    DatePipe,
    MatButtonModule,
    MatButtonToggleModule,
    MatChipsModule,
    MatFormFieldModule,
    MatSelectModule,
    ProductCard,
  ],
  templateUrl: './products.html',
  styleUrl: './products.scss',
})
export class Products implements OnInit {
  private readonly catalogService = inject(CatalogService);
  private readonly storeCommunityService = inject(StoreCommunityService);
  readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly cartStorageKey = 'playverse_cart';
  private readonly fallbackGames: Product[] = [
    {
      id: 1,
      title: 'Elden Ring',
      description: 'Juego de accion y aventura en mundo abierto.',
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
      description: 'Juego de granja y simulacion.',
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
      description: 'Juego competitivo de disparos por equipos.',
      price: 0,
      category: 'FPS',
      thumbnail: 'https://shared.cloudflare.steamstatic.com/store_item_assets/steam/apps/730/header.jpg',
      rating: 'Popular',
      tags: ['Online', 'Competitivo'],
    },
    {
      id: 4,
      title: 'Dota 2',
      description: 'Juego MOBA con heroes y partidas online.',
      price: 0,
      category: 'MOBA',
      thumbnail: 'https://shared.cloudflare.steamstatic.com/store_item_assets/steam/apps/570/header.jpg',
      rating: 'Muy positivo',
      tags: ['Estrategia', 'Online'],
    },
  ];

  readonly selectedCategory = signal('Todos');
  readonly games = signal<Product[]>(this.fallbackGames);
  readonly selectedProduct = signal<Product | null>(null);
  readonly selectedGame = signal<Game | null>(null);
  readonly detailLoading = signal(false);
  readonly detailError = signal('');
  readonly loading = signal(false);
  readonly errorMessage = signal('');
  readonly successMessage = signal('');
  readonly cart = signal<Product[]>(this.loadCart());
  readonly library = signal<LibraryItem[]>([]);
  readonly wishlist = signal<WishlistItem[]>([]);
  readonly selectedPaymentMethod = signal('Tarjeta de credito');
  readonly checkoutLoading = signal(false);
  readonly wishlistLoading = signal(false);

  readonly categories = computed(() => [
    'Todos',
    ...new Set(this.games().map((game) => game.category)),
  ]);

  readonly filteredGames = computed(() => {
    const category = this.selectedCategory();
    return category === 'Todos'
      ? this.games()
      : this.games().filter((game) => game.category === category);
  });

  readonly detailImage = computed(() => {
    const game = this.selectedGame();
    const product = this.selectedProduct();

    return game?.imageUrl?.trim() || product?.thumbnail || artworkForTitle(game?.title ?? product?.title ?? '');
  });

  readonly activeOffer = computed(() => {
    const offers = this.selectedGame()?.offers ?? [];
    return offers.find((offer) => offer.isActive) ?? offers[0] ?? null;
  });

  readonly cartTotal = computed(() =>
    this.cart().reduce((total, item) => total + Number(item.price ?? 0), 0),
  );
  readonly ownedGameIds = computed(() => new Set(this.library().map((item) => item.game.gameId)));
  readonly wishlistGameIds = computed(() => new Set(this.wishlist().map((item) => item.game.gameId)));

  ngOnInit(): void {
    this.loadGames();

    if (this.authService.isAuthenticated()) {
      this.loadPlayerCollections();
    }
  }

  selectCategory(category: string): void {
    this.selectedCategory.set(category);
  }

  showDetail(product: Product): void {
    this.selectedProduct.set(product);
    this.selectedGame.set(null);
    this.detailError.set('');
    this.errorMessage.set('');
    this.successMessage.set('');

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

  selectPaymentMethod(method: string): void {
    this.selectedPaymentMethod.set(method);
  }

  addToCart(product: Product): void {
    if (!this.authService.isAuthenticated()) {
      this.errorMessage.set('Inicia sesion para agregar juegos al carrito.');
      this.router.navigate(['/login']);
      return;
    }

    if (typeof product.id !== 'string') {
      this.errorMessage.set('Este juego es de muestra. Enciende la API local para comprar juegos reales.');
      return;
    }

    if (this.isPurchased(product)) {
      this.errorMessage.set('Ya tienes este juego en tu biblioteca.');
      return;
    }

    const exists = this.cart().some((item) => item.id === product.id);

    if (exists) {
      this.successMessage.set('Este juego ya esta en el carrito.');
      return;
    }

    const nextCart = [...this.cart(), product];
    this.cart.set(nextCart);
    this.persistCart(nextCart);
    this.errorMessage.set('');
    this.successMessage.set(`${product.title} agregado al carrito.`);
  }

  addDetailToWishlist(product: Product): void {
    if (!this.authService.isAuthenticated()) {
      this.errorMessage.set('Inicia sesion para agregar juegos a Wishlist.');
      this.router.navigate(['/login']);
      return;
    }

    if (typeof product.id !== 'string') {
      this.errorMessage.set('Este juego es de muestra. Enciende la API local para usar Wishlist.');
      return;
    }

    if (this.isPurchased(product)) {
      this.errorMessage.set('Ya tienes este juego en tu biblioteca.');
      return;
    }

    if (this.isInWishlist(product)) {
      this.successMessage.set('Este juego ya esta en Wishlist.');
      return;
    }

    this.wishlistLoading.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');

    this.storeCommunityService.addToWishlist(product.id).subscribe({
      next: (item) => {
        this.wishlistLoading.set(false);
        this.wishlist.set([item, ...this.wishlist()]);
        this.successMessage.set('Se añadido a Wishlist');
      },
      error: (error: unknown) => {
        this.wishlistLoading.set(false);
        this.errorMessage.set(this.resolveApiError(error));
      },
    });
  }

  removeFromCart(productId: string | number): void {
    const nextCart = this.cart().filter((item) => item.id !== productId);
    this.cart.set(nextCart);
    this.persistCart(nextCart);
  }

  checkoutCart(): void {
    if (!this.authService.isAuthenticated()) {
      this.errorMessage.set('Inicia sesion para completar la compra.');
      this.router.navigate(['/login']);
      return;
    }

    const cart = this.cart();

    if (!cart.length) {
      this.errorMessage.set('Agrega al menos un juego al carrito.');
      return;
    }

    const invalidItem = cart.find((item) => typeof item.id !== 'string');

    if (invalidItem) {
      this.errorMessage.set('Solo se pueden comprar juegos cargados desde el backend.');
      return;
    }

    const alreadyOwned = cart.find((item) => this.isPurchased(item));

    if (alreadyOwned) {
      this.removeFromCart(alreadyOwned.id);
      this.errorMessage.set(`"${alreadyOwned.title}" ya esta en tu biblioteca y se quito del carrito.`);
      return;
    }

    this.checkoutLoading.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');

    forkJoin(cart.map((item) => this.storeCommunityService.purchaseGame(String(item.id)))).subscribe({
      next: (items) => {
        this.checkoutLoading.set(false);
        this.library.set([...items, ...this.library()]);
        this.cart.set([]);
        this.persistCart([]);
        this.successMessage.set(
          `Compra completada con ${this.selectedPaymentMethod()}. Revisa tu biblioteca en el perfil.`,
        );
      },
      error: (error: unknown) => {
        this.checkoutLoading.set(false);
        this.errorMessage.set(this.resolveApiError(error));
      },
    });
  }

  isInCart(product: Product): boolean {
    return this.cart().some((item) => item.id === product.id);
  }

  isPurchased(product: Product): boolean {
    return typeof product.id === 'string' && this.ownedGameIds().has(product.id);
  }

  canAddToWishlist(product: Product): boolean {
    return this.authService.isAuthenticated() && typeof product.id === 'string' && !this.isPurchased(product);
  }

  isInWishlist(product: Product): boolean {
    return typeof product.id === 'string' && this.wishlistGameIds().has(product.id);
  }

  private loadGames(): void {
    this.loading.set(true);
    this.errorMessage.set('');

    this.catalogService.getGames().subscribe({
      next: (games) => {
        this.games.set(games.length ? games.map(gameCardToProduct) : this.fallbackGames);
        this.loading.set(false);
      },
      error: () => {
        this.errorMessage.set('Mostrando catalogo de respaldo mientras la API local esta apagada.');
        this.loading.set(false);
      },
    });
  }

  private loadPlayerCollections(): void {
    forkJoin({
      library: this.storeCommunityService.getLibrary(),
      wishlist: this.storeCommunityService.getWishlist(),
    }).subscribe({
      next: ({ library, wishlist }) => {
        this.library.set(library);
        this.wishlist.set(wishlist);
        this.cleanCartFromOwnedGames();
      },
      error: () => undefined,
    });
  }

  private loadCart(): Product[] {
    const raw = localStorage.getItem(this.cartStorageKey);

    if (!raw) {
      return [];
    }

    try {
      return JSON.parse(raw) as Product[];
    } catch {
      return [];
    }
  }

  private persistCart(cart: Product[]): void {
    localStorage.setItem(this.cartStorageKey, JSON.stringify(cart));
  }

  private cleanCartFromOwnedGames(): void {
    const nextCart = this.cart().filter((item) => !this.isPurchased(item));

    if (nextCart.length !== this.cart().length) {
      this.cart.set(nextCart);
      this.persistCart(nextCart);
    }
  }

  private resolveApiError(error: unknown): string {
    if (typeof error === 'object' && error !== null && 'error' in error) {
      const apiError = (error as { error?: unknown }).error;

      if (typeof apiError === 'string') {
        return apiError;
      }

      if (typeof apiError === 'object' && apiError !== null) {
        const data = apiError as {
          message?: string;
          title?: string;
          errors?: string[] | Record<string, string[]>;
        };

        if (Array.isArray(data.errors)) {
          return data.errors[0] ?? data.message ?? 'No se pudo completar la operacion.';
        }

        if (data.errors && typeof data.errors === 'object') {
          const firstError = Object.values(data.errors).flat()[0];
          return firstError ?? data.message ?? data.title ?? 'No se pudo completar la operacion.';
        }

        return data.message ?? data.title ?? 'No se pudo completar la operacion.';
      }
    }

    if (error instanceof Error) {
      return error.message;
    }

    return 'No se pudo completar la operacion.';
  }
}
