export interface Developer {
  developerId: string;
  developerName: string;
  email: string;
  createdAt?: string | null;
  isDeleted: boolean;
}

export interface Publisher {
  publisherId: string;
  publisherName: string;
  createdAt?: string | null;
  isDeleted: boolean;
}

export interface Genre {
  genreId: number;
  name: string;
}

export interface AchievementDto {
  achievementId: number;
  gameId?: string | null;
  name: string;
  description: string;
}

export interface DlcDto {
  dlcId: number;
  gameId?: string | null;
  name: string;
  price: number;
  addedAt?: string | null;
}

export interface OfferDto {
  offerId: number;
  gameId?: string | null;
  discount: number;
  startDate?: string | null;
  endDate?: string | null;
  isActive: boolean;
}

export interface GameCard {
  gameId: string;
  title: string;
  description: string;
  imageUrl?: string | null;
  releaseDate?: string | null;
  basePrice: number;
  finalPrice: number;
  hasActiveOffer: boolean;
  discountPercentage: number;
  developerName: string;
  publisherName: string;
  genres: string[];
}

export interface Game extends GameCard {
  developerId?: string | null;
  publisherId?: string | null;
  genreItems: Genre[];
  achievements: AchievementDto[];
  dlcs: DlcDto[];
  offers: OfferDto[];
  reviewCount: number;
  recommendedCount: number;
  recommendationRate: number;
}

export interface GamePayload {
  title: string;
  description?: string | null;
  imageUrl?: string | null;
  releaseDate?: string | null;
  price: number;
  developerId?: string | null;
  publisherId?: string | null;
  genreIds: number[];
}

export interface DeveloperPayload {
  developerName: string;
  email?: string | null;
  password?: string | null;
}

export interface PublisherPayload {
  publisherName: string;
}

export interface GenrePayload {
  name: string;
}

export interface AchievementPayload {
  name: string;
  description?: string | null;
}

export interface DlcPayload {
  name: string;
  price: number;
}

export interface OfferPayload {
  discount: number;
  startDate: string;
  endDate: string;
}
