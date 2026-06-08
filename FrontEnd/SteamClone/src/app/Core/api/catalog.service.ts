import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { environment } from '../../../environment/environment';
import { ApiResponse } from './models/api.interface';
import {
  AchievementDto,
  AchievementPayload,
  Developer,
  DeveloperPayload,
  DlcDto,
  DlcPayload,
  Game,
  GameCard,
  GamePayload,
  Genre,
  GenrePayload,
  OfferDto,
  OfferPayload,
  Publisher,
  PublisherPayload,
} from './models/catalog.interface';

@Injectable({ providedIn: 'root' })
export class CatalogService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl;

  getGames(search = ''): Observable<GameCard[]> {
    let params = new HttpParams().set('limit', 100);

    if (search.trim()) {
      params = params.set('search', search.trim());
    }

    return this.http
      .get<ApiResponse<GameCard[]>>(`${this.apiUrl}/games`, { params })
      .pipe(map((response) => response.data ?? []));
  }

  getGame(id: string): Observable<Game> {
    return this.http
      .get<ApiResponse<Game>>(`${this.apiUrl}/games/${id}`)
      .pipe(map((response) => response.data));
  }

  createGame(payload: GamePayload): Observable<Game> {
    return this.http
      .post<ApiResponse<Game>>(`${this.apiUrl}/games`, payload)
      .pipe(map((response) => response.data));
  }

  updateGame(id: string, payload: GamePayload): Observable<Game> {
    return this.http
      .put<ApiResponse<Game>>(`${this.apiUrl}/games/${id}`, payload)
      .pipe(map((response) => response.data));
  }

  deleteGame(id: string): Observable<boolean> {
    return this.http
      .delete<ApiResponse<boolean>>(`${this.apiUrl}/games/${id}`)
      .pipe(map((response) => response.data));
  }

  getDevelopers(): Observable<Developer[]> {
    const params = new HttpParams().set('limit', 100);

    return this.http
      .get<ApiResponse<Developer[]>>(`${this.apiUrl}/developers`, { params })
      .pipe(map((response) => response.data ?? []));
  }

  createDeveloper(payload: DeveloperPayload): Observable<Developer> {
    return this.http
      .post<ApiResponse<Developer>>(`${this.apiUrl}/developers`, payload)
      .pipe(map((response) => response.data));
  }

  updateDeveloper(id: string, payload: DeveloperPayload): Observable<Developer> {
    return this.http
      .put<ApiResponse<Developer>>(`${this.apiUrl}/developers/${id}`, payload)
      .pipe(map((response) => response.data));
  }

  deleteDeveloper(id: string): Observable<boolean> {
    return this.http
      .delete<ApiResponse<boolean>>(`${this.apiUrl}/developers/${id}`)
      .pipe(map((response) => response.data));
  }

  getPublishers(): Observable<Publisher[]> {
    const params = new HttpParams().set('limit', 100);

    return this.http
      .get<ApiResponse<Publisher[]>>(`${this.apiUrl}/publishers`, { params })
      .pipe(map((response) => response.data ?? []));
  }

  createPublisher(payload: PublisherPayload): Observable<Publisher> {
    return this.http
      .post<ApiResponse<Publisher>>(`${this.apiUrl}/publishers`, payload)
      .pipe(map((response) => response.data));
  }

  updatePublisher(id: string, payload: PublisherPayload): Observable<Publisher> {
    return this.http
      .put<ApiResponse<Publisher>>(`${this.apiUrl}/publishers/${id}`, payload)
      .pipe(map((response) => response.data));
  }

  deletePublisher(id: string): Observable<boolean> {
    return this.http
      .delete<ApiResponse<boolean>>(`${this.apiUrl}/publishers/${id}`)
      .pipe(map((response) => response.data));
  }

  getGenres(): Observable<Genre[]> {
    const params = new HttpParams().set('limit', 100);

    return this.http
      .get<ApiResponse<Genre[]>>(`${this.apiUrl}/genres`, { params })
      .pipe(map((response) => response.data ?? []));
  }

  createGenre(payload: GenrePayload): Observable<Genre> {
    return this.http
      .post<ApiResponse<Genre>>(`${this.apiUrl}/genres`, payload)
      .pipe(map((response) => response.data));
  }

  updateGenre(id: number, payload: GenrePayload): Observable<Genre> {
    return this.http
      .put<ApiResponse<Genre>>(`${this.apiUrl}/genres/${id}`, payload)
      .pipe(map((response) => response.data));
  }

  deleteGenre(id: number): Observable<boolean> {
    return this.http
      .delete<ApiResponse<boolean>>(`${this.apiUrl}/genres/${id}`)
      .pipe(map((response) => response.data));
  }

  addAchievement(gameId: string, payload: AchievementPayload): Observable<AchievementDto> {
    return this.http
      .post<ApiResponse<AchievementDto>>(`${this.apiUrl}/games/${gameId}/achievements`, payload)
      .pipe(map((response) => response.data));
  }

  addDlc(gameId: string, payload: DlcPayload): Observable<DlcDto> {
    return this.http
      .post<ApiResponse<DlcDto>>(`${this.apiUrl}/games/${gameId}/dlcs`, payload)
      .pipe(map((response) => response.data));
  }

  addOffer(gameId: string, payload: OfferPayload): Observable<OfferDto> {
    return this.http
      .post<ApiResponse<OfferDto>>(`${this.apiUrl}/games/${gameId}/offers`, payload)
      .pipe(map((response) => response.data));
  }
}
