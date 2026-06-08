import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { environment } from '../../../environment/environment';
import { ApiResponse } from './models/api.interface';
import {
  Friend,
  GameSession,
  LibraryItem,
  Review,
  ReviewAnswer,
  UserAchievement,
  WishlistItem,
} from './models/community.interface';

@Injectable({ providedIn: 'root' })
export class StoreCommunityService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl;

  getWishlist(): Observable<WishlistItem[]> {
    return this.http
      .get<ApiResponse<WishlistItem[]>>(`${this.apiUrl}/wishlist`)
      .pipe(map((response) => response.data ?? []));
  }

  addToWishlist(gameId: string): Observable<WishlistItem> {
    return this.http
      .post<ApiResponse<WishlistItem>>(`${this.apiUrl}/wishlist`, { gameId })
      .pipe(map((response) => response.data));
  }

  removeFromWishlist(gameId: string): Observable<boolean> {
    return this.http
      .delete<ApiResponse<boolean>>(`${this.apiUrl}/wishlist/${gameId}`)
      .pipe(map((response) => response.data));
  }

  getLibrary(): Observable<LibraryItem[]> {
    return this.http
      .get<ApiResponse<LibraryItem[]>>(`${this.apiUrl}/library`)
      .pipe(map((response) => response.data ?? []));
  }

  purchaseGame(gameId: string): Observable<LibraryItem> {
    return this.http
      .post<ApiResponse<LibraryItem>>(`${this.apiUrl}/library/purchase`, { gameId })
      .pipe(map((response) => response.data));
  }

  getReviews(gameId?: string): Observable<Review[]> {
    const params = gameId ? new HttpParams().set('gameId', gameId) : undefined;

    return this.http
      .get<ApiResponse<Review[]>>(`${this.apiUrl}/reviews`, { params })
      .pipe(map((response) => response.data ?? []));
  }

  createReview(gameId: string, isRecommended: boolean, comment: string): Observable<Review> {
    return this.http
      .post<ApiResponse<Review>>(`${this.apiUrl}/reviews`, { gameId, isRecommended, comment })
      .pipe(map((response) => response.data));
  }

  updateReview(reviewId: number, isRecommended: boolean, comment: string): Observable<Review> {
    return this.http
      .put<ApiResponse<Review>>(`${this.apiUrl}/reviews/${reviewId}`, { isRecommended, comment })
      .pipe(map((response) => response.data));
  }

  deleteReview(reviewId: number): Observable<boolean> {
    return this.http
      .delete<ApiResponse<boolean>>(`${this.apiUrl}/reviews/${reviewId}`)
      .pipe(map((response) => response.data));
  }

  answerReview(reviewId: number, comment: string): Observable<ReviewAnswer> {
    return this.http
      .post<ApiResponse<ReviewAnswer>>(`${this.apiUrl}/reviews/${reviewId}/answers`, { comment })
      .pipe(map((response) => response.data));
  }

  getSessions(onlyActive = false): Observable<GameSession[]> {
    const params = onlyActive ? new HttpParams().set('onlyActive', true) : undefined;

    return this.http
      .get<ApiResponse<GameSession[]>>(`${this.apiUrl}/sessions`, { params })
      .pipe(map((response) => response.data ?? []));
  }

  startSession(gameId: string): Observable<GameSession> {
    return this.http
      .post<ApiResponse<GameSession>>(`${this.apiUrl}/sessions/start`, { gameId })
      .pipe(map((response) => response.data));
  }

  endSession(sessionId: number): Observable<GameSession> {
    return this.http
      .post<ApiResponse<GameSession>>(`${this.apiUrl}/sessions/${sessionId}/end`, {})
      .pipe(map((response) => response.data));
  }

  getAchievements(gameId?: string): Observable<UserAchievement[]> {
    const params = gameId ? new HttpParams().set('gameId', gameId) : undefined;

    return this.http
      .get<ApiResponse<UserAchievement[]>>(`${this.apiUrl}/userachievements`, { params })
      .pipe(map((response) => response.data ?? []));
  }

  unlockAchievement(achievementId: number): Observable<UserAchievement> {
    return this.http
      .post<ApiResponse<UserAchievement>>(`${this.apiUrl}/userachievements/unlock`, { achievementId })
      .pipe(map((response) => response.data));
  }

  getFriends(): Observable<Friend[]> {
    return this.http
      .get<ApiResponse<Friend[]>>(`${this.apiUrl}/friends`)
      .pipe(map((response) => response.data ?? []));
  }

  addFriend(friendId: string): Observable<Friend> {
    return this.http
      .post<ApiResponse<Friend>>(`${this.apiUrl}/friends`, { friendId })
      .pipe(map((response) => response.data));
  }

  removeFriend(friendId: string): Observable<boolean> {
    return this.http
      .delete<ApiResponse<boolean>>(`${this.apiUrl}/friends/${friendId}`)
      .pipe(map((response) => response.data));
  }
}
