import { GameCard } from './catalog.interface';

export interface WishlistItem {
  wishlistId: number;
  addedAt?: string | null;
  game: GameCard;
}

export interface LibraryItem {
  libraryId: number;
  purchasePrice: number;
  purchaseDate?: string | null;
  game: GameCard;
}

export interface ReviewAnswer {
  reviewAnswerId: number;
  userId: string;
  username: string;
  comment: string;
  createdAt?: string | null;
  updatedAt?: string | null;
}

export interface Review {
  reviewId: number;
  userId: string;
  username: string;
  gameId: string;
  isRecommended: boolean;
  comment: string;
  createdAt?: string | null;
  updatedAt?: string | null;
  answers: ReviewAnswer[];
}

export interface Friend {
  friendId: string;
  username: string;
  email: string;
  addedAt?: string | null;
}

export interface GameSession {
  sessionId: number;
  gameId: string;
  gameTitle: string;
  startTime?: string | null;
  endTime?: string | null;
  durationMinutes: number;
}

export interface UserAchievement {
  userAchievementId: number;
  achievementId: number;
  achievementName: string;
  achievementDescription: string;
  gameId?: string | null;
  gameTitle: string;
  unlockedAt?: string | null;
}
