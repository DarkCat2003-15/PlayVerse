import { CurrencyPipe, DatePipe } from '@angular/common';
import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { Router } from '@angular/router';
import { Observable, forkJoin } from 'rxjs';
import {
  ConfirmDialog,
  ConfirmDialogData,
} from '../../../../shared/ui/confirm-dialog/confirm-dialog';
import { CatalogService } from '../../../../Core/api/catalog.service';
import {
  AchievementPayload,
  Developer,
  DeveloperPayload,
  DlcPayload,
  Game,
  GameCard,
  GamePayload,
  Genre,
  GenrePayload,
  OfferPayload,
  Publisher,
  PublisherPayload,
} from '../../../../Core/api/models/catalog.interface';
import {
  Friend,
  GameSession,
  LibraryItem,
  Review,
  UserAchievement,
  WishlistItem,
} from '../../../../Core/api/models/community.interface';
import { Permission, Role, UserAccount } from '../../../../Core/api/models/roles.interface';
import { RolesService } from '../../../../Core/api/roles.service';
import { StoreCommunityService } from '../../../../Core/api/store-community.service';
import { AuthService } from '../../../../Core/auth/auth.service';

type SectionKey = 'profile' | 'overview' | 'catalog' | 'content' | 'store' | 'community' | 'access' | 'account';
type CatalogEntityKey = 'developer' | 'publisher' | 'genre';

interface DashboardSection {
  key: SectionKey;
  label: string;
  description: string;
  marker: string;
}

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [
    ConfirmDialog,
    CurrencyPipe,
    DatePipe,
    MatButtonModule,
    MatCardModule,
    MatChipsModule,
    MatFormFieldModule,
    MatInputModule,
    MatProgressSpinnerModule,
    MatSelectModule,
    ReactiveFormsModule,
  ],
  templateUrl: './admin-dashboard.html',
  styleUrl: './admin-dashboard.scss',
})
export class AdminDashboard implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly catalogService = inject(CatalogService);
  private readonly storeCommunityService = inject(StoreCommunityService);
  private readonly rolesService = inject(RolesService);
  private readonly router = inject(Router);
  readonly authService = inject(AuthService);

  readonly sections: DashboardSection[] = [
    {
      key: 'profile',
      label: 'Perfil',
      description: 'Biblioteca y amigos',
      marker: '01',
    },
    {
      key: 'overview',
      label: 'Resumen',
      description: 'Estado general',
      marker: '02',
    },
    {
      key: 'catalog',
      label: 'Catalogo',
      description: 'Juegos y maestros',
      marker: '03',
    },
    {
      key: 'content',
      label: 'Contenido',
      description: 'Logros, DLC y ofertas',
      marker: '04',
    },
    {
      key: 'store',
      label: 'Tienda',
      description: 'Wishlist y libreria',
      marker: '05',
    },
    {
      key: 'community',
      label: 'Comunidad',
      description: 'Reviews y sesiones',
      marker: '06',
    },
    {
      key: 'access',
      label: 'Accesos',
      description: 'Roles y usuarios',
      marker: '07',
    },
    {
      key: 'account',
      label: 'Cuenta',
      description: 'Seguridad y registro',
      marker: '08',
    },
  ];

  readonly activeSection = signal<SectionKey>('profile');
  readonly catalogEntityTab = signal<CatalogEntityKey>('developer');

  readonly games = signal<GameCard[]>([]);
  readonly developers = signal<Developer[]>([]);
  readonly publishers = signal<Publisher[]>([]);
  readonly genres = signal<Genre[]>([]);
  readonly selectedGame = signal<Game | null>(null);
  readonly deleteCandidate = signal<GameCard | null>(null);
  readonly selectedLibraryItem = signal<LibraryItem | null>(null);
  readonly selectedLibraryGame = signal<Game | null>(null);

  readonly wishlist = signal<WishlistItem[]>([]);
  readonly library = signal<LibraryItem[]>([]);
  readonly reviews = signal<Review[]>([]);
  readonly sessions = signal<GameSession[]>([]);
  readonly achievements = signal<UserAchievement[]>([]);
  readonly friends = signal<Friend[]>([]);

  readonly roles = signal<Role[]>([]);
  readonly permissions = signal<Permission[]>([]);
  readonly users = signal<UserAccount[]>([]);

  readonly loading = signal(false);
  readonly playerLoading = signal(false);
  readonly accessLoading = signal(false);
  readonly saving = signal(false);
  readonly errorMessage = signal('');
  readonly successMessage = signal('');
  readonly editingId = signal<string | null>(null);

  readonly userRoles = computed(() =>
    (this.authService.currentUser()?.roles ?? []).map((role) => role.toLowerCase()),
  );
  readonly userPermissions = computed(() => this.authService.currentUser()?.permissions ?? []);

  readonly isAdmin = computed(() => this.hasRoleValue('admin') || this.hasPermissionValue('ROLES_ASSIGN'));
  readonly isDeveloper = computed(() => this.hasRoleValue('developer') || this.hasPermissionValue('GAMES_MANAGE'));
  readonly isPlayer = computed(() => this.authService.isAuthenticated());
  readonly isUserOnly = computed(() => this.hasRoleValue('user') && !this.isAdmin() && !this.isDeveloper());
  readonly canManageCatalog = computed(
    () =>
      this.isAdmin() ||
      this.isDeveloper() ||
      this.hasPermissionValue('CATALOG_MANAGE') ||
      this.hasPermissionValue('GAMES_MANAGE'),
  );
  readonly canUseStore = computed(
    () =>
      this.isPlayer() &&
      (this.hasRoleValue('user') ||
        this.hasRoleValue('developer') ||
        this.isAdmin() ||
        this.hasPermissionValue('WISHLIST_MANAGE')),
  );

  readonly visibleSections = computed(() =>
    this.sections.filter((section) => {
      if (section.key === 'profile') {
        return this.isPlayer();
      }

      if (section.key === 'overview') {
        return !this.isUserOnly();
      }

      if (section.key === 'access') {
        return this.isAdmin() || this.hasPermissionValue('USERS_READ');
      }

      if (section.key === 'catalog' || section.key === 'content') {
        return this.canManageCatalog();
      }

      if (section.key === 'store' || section.key === 'community') {
        return this.canUseStore();
      }

      return true;
    }),
  );

  readonly totalGames = computed(() => this.games().length);
  readonly discountedGames = computed(() => this.games().filter((game) => game.hasActiveOffer).length);
  readonly freeGames = computed(() => this.games().filter((game) => Number(game.finalPrice) === 0).length);
  readonly catalogValue = computed(() =>
    this.games().reduce((total, game) => total + Number(game.finalPrice ?? game.basePrice ?? 0), 0),
  );
  readonly activeSessions = computed(() => this.sessions().filter((session) => !session.endTime).length);
  readonly recommendedReviews = computed(() => this.reviews().filter((review) => review.isRecommended).length);
  readonly availableAchievements = computed(() => this.selectedGame()?.achievements ?? []);
  readonly reviewableGames = computed<GameCard[]>(() =>
    this.isUserOnly() ? this.library().map((item) => item.game) : this.games(),
  );
  readonly myReviews = computed(() => {
    const userId = this.authService.currentUser()?.userId;
    const reviews = this.reviews();

    return userId ? reviews.filter((review) => review.userId === userId) : reviews;
  });
  readonly myActiveSessions = computed(() => this.sessions().filter((session) => !session.endTime));
  readonly selectedLibraryOffer = computed(() => {
    const offers = this.selectedLibraryGame()?.offers ?? [];
    return offers.find((offer) => offer.isActive) ?? offers[0] ?? null;
  });

  readonly deleteDialogData = computed<ConfirmDialogData>(() => {
    const game = this.deleteCandidate();

    return {
      title: 'Eliminar juego',
      message: game
        ? `Esta accion eliminara "${game.title}" del catalogo.`
        : 'Esta seguro de continuar?',
      cancelText: 'Cancelar',
      confirmText: 'Eliminar',
    };
  });

  readonly gameForm = this.fb.nonNullable.group({
    title: ['', [Validators.required, Validators.maxLength(200)]],
    description: ['', [Validators.maxLength(1000)]],
    imageUrl: ['', [Validators.maxLength(600)]],
    releaseDate: [''],
    price: [0, [Validators.required, Validators.min(0)]],
    developerId: [''],
    publisherId: [''],
    genreIds: [[] as number[]],
  });

  readonly developerForm = this.fb.nonNullable.group({
    developerId: [''],
    developerName: ['', [Validators.required, Validators.maxLength(150)]],
    email: ['', [Validators.email, Validators.maxLength(150)]],
    password: ['', [Validators.maxLength(255)]],
  });

  readonly publisherForm = this.fb.nonNullable.group({
    publisherId: [''],
    publisherName: ['', [Validators.required, Validators.maxLength(150)]],
  });

  readonly genreForm = this.fb.nonNullable.group({
    genreId: [0],
    name: ['', [Validators.required, Validators.maxLength(100)]],
  });

  readonly contentGameForm = this.fb.nonNullable.group({
    gameId: ['', Validators.required],
  });

  readonly achievementForm = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(200)]],
    description: ['', [Validators.maxLength(500)]],
  });

  readonly dlcForm = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(200)]],
    price: [0, [Validators.required, Validators.min(0)]],
  });

  readonly offerForm = this.fb.nonNullable.group({
    discount: [10, [Validators.required, Validators.min(1), Validators.max(100)]],
    startDate: [this.today()],
    endDate: [this.nextWeek()],
  });

  readonly storeGameForm = this.fb.nonNullable.group({
    gameId: [''],
  });

  readonly reviewForm = this.fb.nonNullable.group({
    isRecommended: [true],
    comment: ['', [Validators.maxLength(1000)]],
  });

  readonly reviewUpdateForm = this.fb.nonNullable.group({
    reviewId: [0, [Validators.required, Validators.min(1)]],
    isRecommended: [true],
    comment: ['', [Validators.maxLength(1000)]],
  });

  readonly reviewAnswerForm = this.fb.nonNullable.group({
    reviewId: [0, [Validators.required, Validators.min(1)]],
    comment: ['', [Validators.required, Validators.maxLength(1000)]],
  });

  readonly sessionEndForm = this.fb.nonNullable.group({
    sessionId: [0, [Validators.required, Validators.min(1)]],
  });

  readonly achievementUnlockForm = this.fb.nonNullable.group({
    achievementId: [0, [Validators.required, Validators.min(1)]],
  });

  readonly friendForm = this.fb.nonNullable.group({
    friendId: ['', Validators.required],
  });

  readonly userSearchForm = this.fb.nonNullable.group({
    search: [''],
  });

  readonly roleAssignForm = this.fb.nonNullable.group({
    userId: ['', Validators.required],
    roleId: ['', Validators.required],
  });

  readonly accountForm = this.fb.nonNullable.group({
    username: ['', [Validators.required, Validators.minLength(5)]],
    email: ['', [Validators.required, Validators.email, Validators.minLength(10)]],
  });

  readonly changePasswordForm = this.fb.nonNullable.group({
    currentPassword: ['', Validators.required],
    newPassword: ['', [Validators.required, Validators.minLength(6)]],
  });

  readonly registerCompleteForm = this.fb.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    username: ['', Validators.required],
    password: ['', [Validators.required, Validators.minLength(8)]],
  });

  ngOnInit(): void {
    this.patchAccountForm();
    this.ensureVisibleSection();
    this.loadPanel();
    this.loadRoleAwareData();

    this.authService.getCurrentUser().subscribe({
      next: () => {
        this.patchAccountForm();
        this.ensureVisibleSection();
        this.loadRoleAwareData();
      },
      error: () => undefined,
    });
  }

  setSection(section: SectionKey): void {
    this.activeSection.set(section);

    if (section === 'access' && (this.isAdmin() || this.hasPermissionValue('USERS_READ'))) {
      this.loadAccess();
    }

    if ((section === 'profile' || section === 'store' || section === 'community') && this.canUseStore()) {
      this.loadPlayerData();
    }
  }

  setCatalogEntityTab(tab: CatalogEntityKey): void {
    this.catalogEntityTab.set(tab);
  }

  loadPanel(): void {
    this.loading.set(true);
    this.errorMessage.set('');

    forkJoin({
      games: this.catalogService.getGames(),
      developers: this.catalogService.getDevelopers(),
      publishers: this.catalogService.getPublishers(),
      genres: this.catalogService.getGenres(),
    }).subscribe({
      next: ({ games, developers, publishers, genres }) => {
        this.games.set(games);
        this.developers.set(developers);
        this.publishers.set(publishers);
        this.genres.set(genres);
        this.loading.set(false);
      },
      error: (error: unknown) => {
        this.errorMessage.set(this.resolveApiError(error));
        this.loading.set(false);
      },
    });
  }

  loadRoleAwareData(): void {
    this.ensureVisibleSection();

    if (this.canUseStore()) {
      this.loadPlayerData();
    }

    if (this.isAdmin() || this.hasPermissionValue('USERS_READ')) {
      this.loadAccess();
    }
  }

  loadPlayerData(): void {
    this.playerLoading.set(true);

    forkJoin({
      wishlist: this.storeCommunityService.getWishlist(),
      library: this.storeCommunityService.getLibrary(),
      reviews: this.storeCommunityService.getReviews(),
      sessions: this.storeCommunityService.getSessions(),
      achievements: this.storeCommunityService.getAchievements(),
      friends: this.storeCommunityService.getFriends(),
    }).subscribe({
      next: ({ wishlist, library, reviews, sessions, achievements, friends }) => {
        this.wishlist.set(wishlist);
        this.library.set(library);
        this.reviews.set(reviews);
        this.sessions.set(sessions);
        this.achievements.set(achievements);
        this.friends.set(friends);
        this.playerLoading.set(false);
      },
      error: (error: unknown) => {
        this.playerLoading.set(false);
        this.errorMessage.set(this.resolveApiError(error));
      },
    });
  }

  loadReviewsForSelectedGame(): void {
    const gameId = this.storeGameForm.controls.gameId.value;

    if (!gameId) {
      this.loadPlayerData();
      return;
    }

    this.storeCommunityService.getReviews(gameId).subscribe({
      next: (reviews) => this.reviews.set(reviews),
      error: (error: unknown) => this.errorMessage.set(this.resolveApiError(error)),
    });
  }

  loadAccess(): void {
    this.accessLoading.set(true);

    forkJoin({
      roles: this.rolesService.getRoles(),
      permissions: this.rolesService.getPermissions(),
      users: this.rolesService.getUsers(this.userSearchForm.controls.search.value),
    }).subscribe({
      next: ({ roles, permissions, users }) => {
        this.roles.set(roles);
        this.permissions.set(permissions);
        this.users.set(users);
        this.accessLoading.set(false);
      },
      error: (error: unknown) => {
        this.accessLoading.set(false);
        this.errorMessage.set(this.resolveApiError(error));
      },
    });
  }

  editGame(game: GameCard): void {
    this.successMessage.set('');
    this.errorMessage.set('');
    this.editingId.set(game.gameId);
    this.activeSection.set('catalog');

    this.catalogService.getGame(game.gameId).subscribe({
      next: (detail) => {
        this.selectedGame.set(detail);
        this.gameForm.patchValue({
          title: detail.title,
          description: detail.description ?? '',
          imageUrl: detail.imageUrl ?? '',
          releaseDate: detail.releaseDate ?? '',
          price: Number(detail.basePrice ?? detail.finalPrice ?? 0),
          developerId: detail.developerId ?? '',
          publisherId: detail.publisherId ?? '',
          genreIds: detail.genreItems.map((genre) => genre.genreId),
        });
      },
      error: (error: unknown) => {
        this.errorMessage.set(this.resolveApiError(error));
      },
    });
  }

  saveGame(): void {
    if (this.gameForm.invalid) {
      this.gameForm.markAllAsTouched();
      return;
    }

    const editingId = this.editingId();
    const request$ = editingId
      ? this.catalogService.updateGame(editingId, this.buildPayload())
      : this.catalogService.createGame(this.buildPayload());

    this.runMutation(
      request$,
      editingId ? 'Juego actualizado correctamente.' : 'Juego creado correctamente.',
      () => {
        this.cancelEdit();
        this.loadPanel();
      },
    );
  }

  cancelEdit(): void {
    this.editingId.set(null);
    this.selectedGame.set(null);
    this.gameForm.reset({
      title: '',
      description: '',
      imageUrl: '',
      releaseDate: '',
      price: 0,
      developerId: '',
      publisherId: '',
      genreIds: [],
    });
  }

  askDelete(game: GameCard): void {
    this.deleteCandidate.set(game);
  }

  confirmDelete(confirmed: boolean): void {
    const game = this.deleteCandidate();
    this.deleteCandidate.set(null);

    if (!confirmed || !game) {
      return;
    }

    this.runMutation(this.catalogService.deleteGame(game.gameId), `"${game.title}" fue eliminado.`, () =>
      this.loadPanel(),
    );
  }

  editDeveloper(developer: Developer): void {
    this.catalogEntityTab.set('developer');
    this.developerForm.patchValue({
      developerId: developer.developerId,
      developerName: developer.developerName,
      email: developer.email ?? '',
      password: '',
    });
  }

  saveDeveloper(): void {
    if (this.developerForm.invalid) {
      this.developerForm.markAllAsTouched();
      return;
    }

    const raw = this.developerForm.getRawValue();
    const payload: DeveloperPayload = {
      developerName: raw.developerName.trim(),
      email: raw.email.trim() || null,
      password: raw.password.trim() || null,
    };
    const request$ = raw.developerId
      ? this.catalogService.updateDeveloper(raw.developerId, payload)
      : this.catalogService.createDeveloper(payload);

    this.runMutation(request$, raw.developerId ? 'Desarrollador actualizado.' : 'Desarrollador creado.', () => {
      this.resetDeveloperForm();
      this.loadPanel();
    });
  }

  deleteDeveloper(developer: Developer): void {
    this.runMutation(
      this.catalogService.deleteDeveloper(developer.developerId),
      `Desarrollador "${developer.developerName}" eliminado.`,
      () => this.loadPanel(),
    );
  }

  resetDeveloperForm(): void {
    this.developerForm.reset({
      developerId: '',
      developerName: '',
      email: '',
      password: '',
    });
  }

  editPublisher(publisher: Publisher): void {
    this.catalogEntityTab.set('publisher');
    this.publisherForm.patchValue({
      publisherId: publisher.publisherId,
      publisherName: publisher.publisherName,
    });
  }

  savePublisher(): void {
    if (this.publisherForm.invalid) {
      this.publisherForm.markAllAsTouched();
      return;
    }

    const raw = this.publisherForm.getRawValue();
    const payload: PublisherPayload = {
      publisherName: raw.publisherName.trim(),
    };
    const request$ = raw.publisherId
      ? this.catalogService.updatePublisher(raw.publisherId, payload)
      : this.catalogService.createPublisher(payload);

    this.runMutation(request$, raw.publisherId ? 'Publisher actualizado.' : 'Publisher creado.', () => {
      this.resetPublisherForm();
      this.loadPanel();
    });
  }

  deletePublisher(publisher: Publisher): void {
    this.runMutation(
      this.catalogService.deletePublisher(publisher.publisherId),
      `Publisher "${publisher.publisherName}" eliminado.`,
      () => this.loadPanel(),
    );
  }

  resetPublisherForm(): void {
    this.publisherForm.reset({
      publisherId: '',
      publisherName: '',
    });
  }

  editGenre(genre: Genre): void {
    this.catalogEntityTab.set('genre');
    this.genreForm.patchValue({
      genreId: genre.genreId,
      name: genre.name,
    });
  }

  saveGenre(): void {
    if (this.genreForm.invalid) {
      this.genreForm.markAllAsTouched();
      return;
    }

    const raw = this.genreForm.getRawValue();
    const payload: GenrePayload = {
      name: raw.name.trim(),
    };
    const request$ = raw.genreId
      ? this.catalogService.updateGenre(raw.genreId, payload)
      : this.catalogService.createGenre(payload);

    this.runMutation(request$, raw.genreId ? 'Genero actualizado.' : 'Genero creado.', () => {
      this.resetGenreForm();
      this.loadPanel();
    });
  }

  deleteGenre(genre: Genre): void {
    this.runMutation(this.catalogService.deleteGenre(genre.genreId), `Genero "${genre.name}" eliminado.`, () =>
      this.loadPanel(),
    );
  }

  resetGenreForm(): void {
    this.genreForm.reset({
      genreId: 0,
      name: '',
    });
  }

  selectContentGame(gameId: string): void {
    if (!gameId) {
      this.selectedGame.set(null);
      return;
    }

    this.catalogService.getGame(gameId).subscribe({
      next: (game) => this.selectedGame.set(game),
      error: (error: unknown) => this.errorMessage.set(this.resolveApiError(error)),
    });
  }

  addAchievement(): void {
    if (this.contentGameForm.invalid || this.achievementForm.invalid) {
      this.contentGameForm.markAllAsTouched();
      this.achievementForm.markAllAsTouched();
      return;
    }

    const gameId = this.contentGameForm.controls.gameId.value;
    const raw = this.achievementForm.getRawValue();
    const payload: AchievementPayload = {
      name: raw.name.trim(),
      description: raw.description.trim() || null,
    };

    this.runMutation(this.catalogService.addAchievement(gameId, payload), 'Logro agregado al juego.', () => {
      this.achievementForm.reset({ name: '', description: '' });
      this.selectContentGame(gameId);
    });
  }

  addDlc(): void {
    if (this.contentGameForm.invalid || this.dlcForm.invalid) {
      this.contentGameForm.markAllAsTouched();
      this.dlcForm.markAllAsTouched();
      return;
    }

    const gameId = this.contentGameForm.controls.gameId.value;
    const raw = this.dlcForm.getRawValue();
    const payload: DlcPayload = {
      name: raw.name.trim(),
      price: Number(raw.price ?? 0),
    };

    this.runMutation(this.catalogService.addDlc(gameId, payload), 'DLC agregado al juego.', () => {
      this.dlcForm.reset({ name: '', price: 0 });
      this.selectContentGame(gameId);
    });
  }

  addOffer(): void {
    if (this.contentGameForm.invalid || this.offerForm.invalid) {
      this.contentGameForm.markAllAsTouched();
      this.offerForm.markAllAsTouched();
      return;
    }

    const gameId = this.contentGameForm.controls.gameId.value;
    const raw = this.offerForm.getRawValue();
    const payload: OfferPayload = {
      discount: Number(raw.discount ?? 0),
      startDate: this.asDateTime(raw.startDate),
      endDate: this.asDateTime(raw.endDate),
    };

    this.runMutation(this.catalogService.addOffer(gameId, payload), 'Oferta publicada.', () => {
      this.offerForm.reset({ discount: 10, startDate: this.today(), endDate: this.nextWeek() });
      this.selectContentGame(gameId);
      this.loadPanel();
    });
  }

  addWishlist(): void {
    const gameId = this.requireStoreGame();

    if (!gameId) {
      return;
    }

    this.runMutation(this.storeCommunityService.addToWishlist(gameId), 'Juego agregado a wishlist.', () =>
      this.loadPlayerData(),
    );
  }

  removeWishlist(gameId: string): void {
    this.runMutation(this.storeCommunityService.removeFromWishlist(gameId), 'Juego removido de wishlist.', () =>
      this.loadPlayerData(),
    );
  }

  purchaseGame(): void {
    const gameId = this.requireStoreGame();

    if (!gameId) {
      return;
    }

    if (this.isGameOwned(gameId)) {
      this.errorMessage.set('Ya tienes este juego en tu biblioteca.');
      return;
    }

    this.runMutation(this.storeCommunityService.purchaseGame(gameId), 'Compra registrada en la libreria.', () =>
      this.loadPlayerData(),
    );
  }

  createReview(): void {
    const gameId = this.requireStoreGame();

    if (!gameId || this.reviewForm.invalid) {
      this.reviewForm.markAllAsTouched();
      return;
    }

    const raw = this.reviewForm.getRawValue();

    this.runMutation(
      this.storeCommunityService.createReview(gameId, raw.isRecommended, raw.comment.trim()),
      'Review publicada.',
      () => {
        this.reviewForm.reset({ isRecommended: true, comment: '' });
        this.loadPlayerData();
      },
    );
  }

  updateReview(): void {
    if (this.reviewUpdateForm.invalid) {
      this.reviewUpdateForm.markAllAsTouched();
      return;
    }

    const raw = this.reviewUpdateForm.getRawValue();

    this.runMutation(
      this.storeCommunityService.updateReview(raw.reviewId, raw.isRecommended, raw.comment.trim()),
      'Review actualizada.',
      () => this.loadPlayerData(),
    );
  }

  deleteReview(reviewId: number): void {
    this.runMutation(this.storeCommunityService.deleteReview(reviewId), 'Review eliminada.', () =>
      this.loadPlayerData(),
    );
  }

  answerReview(): void {
    if (this.reviewAnswerForm.invalid) {
      this.reviewAnswerForm.markAllAsTouched();
      return;
    }

    const raw = this.reviewAnswerForm.getRawValue();

    this.runMutation(
      this.storeCommunityService.answerReview(raw.reviewId, raw.comment.trim()),
      'Respuesta agregada.',
      () => {
        this.reviewAnswerForm.reset({ reviewId: 0, comment: '' });
        this.loadPlayerData();
      },
    );
  }

  startSession(): void {
    const gameId = this.requireStoreGame();

    if (!gameId) {
      return;
    }

    this.runMutation(this.storeCommunityService.startSession(gameId), 'Sesion iniciada.', () => this.loadPlayerData());
  }

  viewLibraryItem(item: LibraryItem): void {
    this.selectedLibraryItem.set(item);
    this.selectedLibraryGame.set(null);

    this.catalogService.getGame(item.game.gameId).subscribe({
      next: (game) => this.selectedLibraryGame.set(game),
      error: () => {
        this.selectedLibraryGame.set(null);
      },
    });
  }

  closeLibraryDetail(): void {
    this.selectedLibraryItem.set(null);
    this.selectedLibraryGame.set(null);
  }

  downloadAndStart(item?: LibraryItem): void {
    const target = item ?? this.selectedLibraryItem();

    if (!target) {
      this.errorMessage.set('Selecciona un juego de tu libreria.');
      return;
    }

    this.runMutation(
      this.storeCommunityService.startSession(target.game.gameId),
      `Descarga simulada e inicio de "${target.game.title}" registrados. Ahora aparece como sesion activa.`,
      () => this.loadPlayerData(),
    );
  }

  endSession(): void {
    if (this.sessionEndForm.invalid) {
      this.sessionEndForm.markAllAsTouched();
      return;
    }

    const sessionId = this.sessionEndForm.controls.sessionId.value;

    this.runMutation(this.storeCommunityService.endSession(sessionId), 'Sesion finalizada.', () => {
      this.sessionEndForm.reset({ sessionId: 0 });
      this.loadPlayerData();
    });
  }

  unlockAchievement(): void {
    if (this.achievementUnlockForm.invalid) {
      this.achievementUnlockForm.markAllAsTouched();
      return;
    }

    const achievementId = this.achievementUnlockForm.controls.achievementId.value;

    this.runMutation(this.storeCommunityService.unlockAchievement(achievementId), 'Logro desbloqueado.', () => {
      this.achievementUnlockForm.reset({ achievementId: 0 });
      this.loadPlayerData();
    });
  }

  addFriend(): void {
    if (this.friendForm.invalid) {
      this.friendForm.markAllAsTouched();
      return;
    }

    const friendId = this.friendForm.controls.friendId.value.trim();

    this.runMutation(this.storeCommunityService.addFriend(friendId), 'Amigo agregado.', () => {
      this.friendForm.reset({ friendId: '' });
      this.loadPlayerData();
    });
  }

  removeFriend(friendId: string): void {
    this.runMutation(this.storeCommunityService.removeFriend(friendId), 'Amigo eliminado.', () =>
      this.loadPlayerData(),
    );
  }

  assignRole(): void {
    if (this.roleAssignForm.invalid) {
      this.roleAssignForm.markAllAsTouched();
      return;
    }

    const raw = this.roleAssignForm.getRawValue();

    this.runMutation(this.rolesService.assignRole(raw.userId, raw.roleId), 'Rol asignado al usuario.', () =>
      this.loadAccess(),
    );
  }

  removeRole(userId: string, roleName: string): void {
    const role = this.roles().find((item) => item.name.toLowerCase() === roleName.toLowerCase());

    if (!role) {
      this.errorMessage.set('No se encontro el rol para removerlo.');
      return;
    }

    this.runMutation(this.rolesService.removeRole(userId, role.id), 'Rol removido del usuario.', () =>
      this.loadAccess(),
    );
  }

  updateAccount(): void {
    if (this.accountForm.invalid) {
      this.accountForm.markAllAsTouched();
      return;
    }

    const raw = this.accountForm.getRawValue();

    this.runMutation(
      this.authService.updateAccount({
        username: raw.username.trim(),
        email: raw.email.trim(),
      }),
      'Datos de cuenta actualizados.',
      () => this.patchAccountForm(),
    );
  }

  renewSession(): void {
    this.runMutation(this.authService.renewSession(), 'Sesion renovada correctamente.');
  }

  deleteAccount(): void {
    const user = this.authService.currentUser();

    if (!user) {
      this.errorMessage.set('No hay usuario autenticado.');
      return;
    }

    const confirmed = window.confirm(`Esta accion desactiva la cuenta ${user.email}. Deseas continuar?`);

    if (!confirmed) {
      return;
    }

    this.runMutation(this.authService.deleteAccount(), 'Cuenta eliminada.');
  }

  changePassword(): void {
    if (this.changePasswordForm.invalid) {
      this.changePasswordForm.markAllAsTouched();
      return;
    }

    const raw = this.changePasswordForm.getRawValue();

    this.runMutation(
      this.authService.changePassword(raw.currentPassword, raw.newPassword),
      'Contrasena actualizada.',
      () => this.changePasswordForm.reset({ currentPassword: '', newPassword: '' }),
    );
  }

  registerComplete(): void {
    if (this.registerCompleteForm.invalid) {
      this.registerCompleteForm.markAllAsTouched();
      return;
    }

    const raw = this.registerCompleteForm.getRawValue();

    this.runMutation(
      this.authService.signUp({
        email: raw.email.trim(),
        username: raw.username.trim(),
        password: raw.password,
      }),
      'Usuario registrado correctamente.',
      () => {
        this.registerCompleteForm.reset({
          email: '',
          username: '',
          password: '',
        });
        this.loadAccess();
      },
    );
  }

  logout(): void {
    this.authService.logout();
  }

  gameTitleForId(gameId: string): string {
    return (
      this.games().find((game) => game.gameId === gameId)?.title ??
      this.library().find((item) => item.game.gameId === gameId)?.game.title ??
      gameId
    );
  }

  isGameOwned(gameId: string): boolean {
    return this.library().some((item) => item.game.gameId === gameId);
  }

  private buildPayload(): GamePayload {
    const raw = this.gameForm.getRawValue();

    return {
      title: raw.title.trim(),
      description: raw.description?.trim() || null,
      imageUrl: raw.imageUrl?.trim() || null,
      releaseDate: raw.releaseDate || null,
      price: Number(raw.price ?? 0),
      developerId: raw.developerId || null,
      publisherId: raw.publisherId || null,
      genreIds: raw.genreIds ?? [],
    };
  }

  private requireStoreGame(): string | null {
    const gameId = this.storeGameForm.controls.gameId.value;

    if (!gameId) {
      this.storeGameForm.markAllAsTouched();
      this.errorMessage.set('Selecciona un juego para continuar.');
      return null;
    }

    return gameId;
  }

  private runMutation<T>(request$: Observable<T>, message: string, after?: (response: T) => void): void {
    this.saving.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');

    request$.subscribe({
      next: (response) => {
        this.saving.set(false);
        this.successMessage.set(message);
        after?.(response);
      },
      error: (error: unknown) => {
        this.saving.set(false);
        this.errorMessage.set(this.resolveApiError(error));
      },
    });
  }

  private hasRoleValue(role: string): boolean {
    return this.userRoles().includes(role.toLowerCase());
  }

  private hasPermissionValue(permission: string): boolean {
    return this.userPermissions().includes(permission);
  }

  private ensureVisibleSection(): void {
    const visible = this.visibleSections();
    const preferred = this.router.url.includes('/perfil') || this.isUserOnly() ? 'profile' : 'overview';
    const preferredVisible = visible.some((section) => section.key === preferred);
    const currentVisible = visible.some((section) => section.key === this.activeSection());

    if (currentVisible && !(this.activeSection() === 'profile' && preferred === 'overview')) {
      return;
    }

    this.activeSection.set(preferredVisible ? preferred : visible[0]?.key ?? 'account');
  }

  private patchAccountForm(): void {
    const user = this.authService.currentUser();

    if (!user) {
      return;
    }

    this.accountForm.patchValue({
      username: user.username,
      email: user.email,
    });
  }

  private today(): string {
    return new Date().toISOString().slice(0, 10);
  }

  private nextWeek(): string {
    const date = new Date();
    date.setDate(date.getDate() + 7);
    return date.toISOString().slice(0, 10);
  }

  private asDateTime(value: string): string {
    return value ? `${value}T00:00:00` : new Date().toISOString();
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
