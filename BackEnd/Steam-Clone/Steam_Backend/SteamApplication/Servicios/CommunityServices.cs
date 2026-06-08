using Microsoft.EntityFrameworkCore;
using SteamApplication.Helpers;
using SteamApplication.Interfaces.Servicie;
using SteamApplication.Models.Dtos;
using SteamApplication.Models.Request.Community;
using SteamApplication.Models.Response;
using SteamApplication.Queries;
using SteamDomain.Database.SqlServer.Context;
using SteamDomain.Database.SqlServer.Entities;
using SteamDomain.Exceptions;
using SteamShared.Constants;
using SteamShared.Helpers;

namespace SteamApplication.Servicios
{
    public class WishlistService(SteamContext context) : IWishlistService
    {
        public GenericResponse<List<WishlistItemDto>> Get(Guid userId)
        {
            var now = DateTimeHelper.UtcNow();
            var data = context.Wishlists
                .Include(x => x.Game)
                    .ThenInclude(x => x!.Developer)
                .Include(x => x.Game)
                    .ThenInclude(x => x!.Publisher)
                .Include(x => x.Game)
                    .ThenInclude(x => x!.GameGenres)
                        .ThenInclude(x => x.Genre)
                .Include(x => x.Game)
                    .ThenInclude(x => x!.Offers)
                .Where(x => x.UserId == userId && x.Game != null && x.Game.DeletedAt == null)
                .OrderByDescending(x => x.AddedAt)
                .ToList()
                .Select(x => x.ToDto(now))
                .ToList();

            return ResponseHelper.Create(data, count: data.Count);
        }

        public async Task<GenericResponse<WishlistItemDto>> Add(Guid userId, CreateWishlistRequest model)
        {
            _ = await GetGame(model.GameId);

            var exists = await context.Wishlists.AnyAsync(x => x.UserId == userId && x.GameId == model.GameId);
            if (exists)
            {
                throw new BadRequestException(StoreResponseConstants.WISHLIST_ALREADY_EXISTS);
            }

            var entity = new Wishlist
            {
                UserId = userId,
                GameId = model.GameId,
                AddedAt = DateTimeHelper.UtcNow()
            };

            await context.Wishlists.AddAsync(entity);
            await context.SaveChangesAsync();

            var created = await context.Wishlists
                .Include(x => x.Game)
                    .ThenInclude(x => x!.Developer)
                .Include(x => x.Game)
                    .ThenInclude(x => x!.Publisher)
                .Include(x => x.Game)
                    .ThenInclude(x => x!.GameGenres)
                        .ThenInclude(x => x.Genre)
                .Include(x => x.Game)
                    .ThenInclude(x => x!.Offers)
                .FirstAsync(x => x.WishlistId == entity.WishlistId);

            return ResponseHelper.Create(created.ToDto(DateTimeHelper.UtcNow()));
        }

        public async Task<GenericResponse<bool>> Remove(Guid userId, Guid gameId)
        {
            var entity = await context.Wishlists.FirstOrDefaultAsync(x => x.UserId == userId && x.GameId == gameId)
                ?? throw new NotFoundException(StoreResponseConstants.WISHLIST_NOT_EXISTS);

            context.Wishlists.Remove(entity);
            await context.SaveChangesAsync();

            return ResponseHelper.Create(true);
        }

        private async Task<Game> GetGame(Guid gameId)
        {
            return await context.Games.FirstOrDefaultAsync(x => x.GameId == gameId && x.DeletedAt == null)
                ?? throw new NotFoundException(StoreResponseConstants.GAME_NOT_EXISTS);
        }
    }

    public class LibraryService(SteamContext context) : ILibraryService
    {
        public GenericResponse<List<LibraryItemDto>> Get(Guid userId)
        {
            var now = DateTimeHelper.UtcNow();
            var data = context.Libraries
                .Include(x => x.Game)
                    .ThenInclude(x => x!.Developer)
                .Include(x => x.Game)
                    .ThenInclude(x => x!.Publisher)
                .Include(x => x.Game)
                    .ThenInclude(x => x!.GameGenres)
                        .ThenInclude(x => x.Genre)
                .Include(x => x.Game)
                    .ThenInclude(x => x!.Offers)
                .Where(x => x.UserId == userId && x.Game != null && x.Game.DeletedAt == null)
                .OrderByDescending(x => x.PurchaseDate)
                .ToList()
                .Select(x => x.ToDto(now))
                .ToList();

            return ResponseHelper.Create(data, count: data.Count);
        }

        public async Task<GenericResponse<LibraryItemDto>> Purchase(Guid userId, PurchaseGameRequest model)
        {
            var game = await context.Games
                .Include(x => x.Developer)
                .Include(x => x.Publisher)
                .Include(x => x.GameGenres)
                    .ThenInclude(x => x.Genre)
                .Include(x => x.Offers)
                .FirstOrDefaultAsync(x => x.GameId == model.GameId && x.DeletedAt == null)
                ?? throw new NotFoundException(StoreResponseConstants.GAME_NOT_EXISTS);

            var alreadyOwned = await context.Libraries.AnyAsync(x => x.UserId == userId && x.GameId == model.GameId);
            if (alreadyOwned)
            {
                throw new BadRequestException(StoreResponseConstants.GAME_ALREADY_OWNED);
            }

            var now = DateTimeHelper.UtcNow();
            var entity = new Library
            {
                UserId = userId,
                GameId = model.GameId,
                PurchaseDate = now,
                PurchasePrice = GamePriceHelper.ResolveCurrentPrice(game, now)
            };

            await context.Libraries.AddAsync(entity);

            var wishlistItem = await context.Wishlists.FirstOrDefaultAsync(x => x.UserId == userId && x.GameId == model.GameId);
            if (wishlistItem != null)
            {
                context.Wishlists.Remove(wishlistItem);
            }

            await context.SaveChangesAsync();

            var created = await context.Libraries
                .Include(x => x.Game)
                    .ThenInclude(x => x!.Developer)
                .Include(x => x.Game)
                    .ThenInclude(x => x!.Publisher)
                .Include(x => x.Game)
                    .ThenInclude(x => x!.GameGenres)
                        .ThenInclude(x => x.Genre)
                .Include(x => x.Game)
                    .ThenInclude(x => x!.Offers)
                .FirstAsync(x => x.LibraryId == entity.LibraryId);

            return ResponseHelper.Create(created.ToDto(now));
        }
    }

    public class ReviewService(SteamContext context) : IReviewService
    {
        public GenericResponse<List<ReviewDto>> Get(FilterReviewRequest model)
        {
            var queryable = context.Reviews
                .Include(x => x.User)
                .Include(x => x.ReviewAnswers)
                    .ThenInclude(x => x.User)
                .AsQueryable()
                .ApplyQuery(model);

            var count = queryable.Count();
            var data = queryable
                .OrderByDescending(x => x.CreatedAt)
                .Skip(model.Offset)
                .Take(model.Limit)
                .ToList()
                .Select(x => x.ToDto())
                .ToList();

            return ResponseHelper.Create(data, count: count);
        }

        public async Task<GenericResponse<ReviewDto>> Create(Guid userId, CreateReviewRequest model)
        {
            await EnsureUserOwnsGame(userId, model.GameId);

            var exists = await context.Reviews.AnyAsync(x => x.UserId == userId && x.GameId == model.GameId && x.DeletedAt == null);
            if (exists)
            {
                throw new BadRequestException(StoreResponseConstants.REVIEW_ALREADY_EXISTS);
            }

            var entity = new Review
            {
                UserId = userId,
                GameId = model.GameId,
                IsRecommended = model.IsRecommended,
                Comment = model.Comment,
                CreatedAt = DateTimeHelper.UtcNow(),
                UpdatedAt = DateTimeHelper.UtcNow()
            };

            await context.Reviews.AddAsync(entity);
            await context.SaveChangesAsync();

            var created = await GetReview(entity.ReviewId);
            return ResponseHelper.Create(created.ToDto());
        }

        public async Task<GenericResponse<ReviewDto>> Update(Guid userId, int reviewId, UpdateReviewRequest model)
        {
            var entity = await context.Reviews.FirstOrDefaultAsync(x => x.ReviewId == reviewId && x.DeletedAt == null)
                ?? throw new NotFoundException(StoreResponseConstants.REVIEW_NOT_EXISTS);

            if (entity.UserId != userId)
            {
                throw new UnauthorizedAccessException(ResponseConstants.AUTH_CLAIM_USER_NOT_FOUND);
            }

            entity.IsRecommended = model.IsRecommended;
            entity.Comment = model.Comment;
            entity.UpdatedAt = DateTimeHelper.UtcNow();

            await context.SaveChangesAsync();

            var updated = await GetReview(reviewId);
            return ResponseHelper.Create(updated.ToDto());
        }

        public async Task<GenericResponse<bool>> Delete(Guid userId, int reviewId)
        {
            var entity = await context.Reviews.FirstOrDefaultAsync(x => x.ReviewId == reviewId && x.DeletedAt == null)
                ?? throw new NotFoundException(StoreResponseConstants.REVIEW_NOT_EXISTS);

            if (entity.UserId != userId)
            {
                throw new UnauthorizedAccessException(ResponseConstants.AUTH_CLAIM_USER_NOT_FOUND);
            }

            entity.DeletedAt = DateTimeHelper.UtcNow();
            entity.UpdatedAt = DateTimeHelper.UtcNow();
            await context.SaveChangesAsync();

            return ResponseHelper.Create(true);
        }

        public async Task<GenericResponse<ReviewAnswerDto>> Answer(Guid userId, int reviewId, CreateReviewAnswerRequest model)
        {
            _ = await GetReview(reviewId);

            var entity = new ReviewAnswer
            {
                ReviewId = reviewId,
                UserId = userId,
                Comment = model.Comment,
                CreatedAt = DateTimeHelper.UtcNow(),
                UpdatedAt = DateTimeHelper.UtcNow()
            };

            await context.ReviewAnswers.AddAsync(entity);
            await context.SaveChangesAsync();

            var created = await context.ReviewAnswers
                .Include(x => x.User)
                .FirstAsync(x => x.ReviewAnswersId == entity.ReviewAnswersId && x.UserId == userId);

            return ResponseHelper.Create(created.ToDto());
        }

        private async Task EnsureUserOwnsGame(Guid userId, Guid gameId)
        {
            var ownsGame = await context.Libraries.AnyAsync(x => x.UserId == userId && x.GameId == gameId);
            if (!ownsGame)
            {
                throw new BadRequestException(StoreResponseConstants.GAME_NOT_OWNED);
            }
        }

        private async Task<Review> GetReview(int reviewId)
        {
            return await context.Reviews
                .Include(x => x.User)
                .Include(x => x.ReviewAnswers)
                    .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.ReviewId == reviewId && x.DeletedAt == null)
                ?? throw new NotFoundException(StoreResponseConstants.REVIEW_NOT_EXISTS);
        }
    }

    public class FriendService(SteamContext context) : IFriendService
    {
        public GenericResponse<List<FriendDto>> Get(Guid userId)
        {
            var data = context.Friends
                .Include(x => x.FriendNavigation)
                .Where(x => x.UserId == userId)
                .OrderBy(x => x.FriendNavigation.UserName)
                .ToList()
                .Select(x => x.ToDto())
                .ToList();

            return ResponseHelper.Create(data, count: data.Count);
        }

        public async Task<GenericResponse<FriendDto>> Add(Guid userId, CreateFriendRequest model)
        {
            if (userId == model.FriendId)
            {
                throw new BadRequestException(StoreResponseConstants.FRIEND_SELF_NOT_ALLOWED);
            }

            var friendUser = await context.Users.FirstOrDefaultAsync(x => x.UserId == model.FriendId && x.DeletedAt == null)
                ?? throw new NotFoundException(StoreResponseConstants.FRIEND_NOT_EXISTS);

            var exists = await context.Friends.AnyAsync(x => x.UserId == userId && x.FriendId == model.FriendId);
            if (exists)
            {
                throw new BadRequestException(StoreResponseConstants.FRIEND_ALREADY_EXISTS);
            }

            await context.Friends.AddRangeAsync(
                new Friend
                {
                    UserId = userId,
                    FriendId = model.FriendId,
                    AddedAt = DateTimeHelper.UtcNow()
                },
                new Friend
                {
                    UserId = model.FriendId,
                    FriendId = userId,
                    AddedAt = DateTimeHelper.UtcNow()
                });

            await context.SaveChangesAsync();

            var created = await context.Friends
                .Include(x => x.FriendNavigation)
                .FirstAsync(x => x.UserId == userId && x.FriendId == model.FriendId);

            return ResponseHelper.Create(created.ToDto());
        }

        public async Task<GenericResponse<bool>> Remove(Guid userId, Guid friendId)
        {
            var relations = await context.Friends
                .Where(x =>
                    (x.UserId == userId && x.FriendId == friendId) ||
                    (x.UserId == friendId && x.FriendId == userId))
                .ToListAsync();

            if (relations.Count == 0)
            {
                throw new NotFoundException(StoreResponseConstants.FRIEND_NOT_EXISTS);
            }

            context.Friends.RemoveRange(relations);
            await context.SaveChangesAsync();

            return ResponseHelper.Create(true);
        }
    }

    public class GameSessionService(SteamContext context) : IGameSessionService
    {
        public GenericResponse<List<GameSessionDto>> Get(Guid userId, FilterSessionRequest model)
        {
            var queryable = context.Sessions
                .Include(x => x.Game)
                .AsQueryable()
                .ApplyQuery(userId, model);

            var count = queryable.Count();
            var data = queryable
                .OrderByDescending(x => x.StartTime)
                .Skip(model.Offset)
                .Take(model.Limit)
                .ToList()
                .Select(x => x.ToDto())
                .ToList();

            return ResponseHelper.Create(data, count: count);
        }

        public async Task<GenericResponse<GameSessionDto>> Start(Guid userId, CreateSessionRequest model)
        {
            var ownsGame = await context.Libraries.AnyAsync(x => x.UserId == userId && x.GameId == model.GameId);
            if (!ownsGame)
            {
                throw new BadRequestException(StoreResponseConstants.GAME_NOT_OWNED);
            }

            var activeSession = await context.Sessions.AnyAsync(x => x.UserId == userId && x.EndTime == null);
            if (activeSession)
            {
                throw new BadRequestException(StoreResponseConstants.SESSION_ALREADY_ACTIVE);
            }

            var entity = new Session
            {
                UserId = userId,
                GameId = model.GameId,
                StartTime = DateTimeHelper.UtcNow()
            };

            await context.Sessions.AddAsync(entity);
            await context.SaveChangesAsync();

            var created = await context.Sessions
                .Include(x => x.Game)
                .FirstAsync(x => x.SessionId == entity.SessionId);

            return ResponseHelper.Create(created.ToDto());
        }

        public async Task<GenericResponse<GameSessionDto>> End(Guid userId, int sessionId)
        {
            var entity = await context.Sessions
                .Include(x => x.Game)
                .FirstOrDefaultAsync(x => x.SessionId == sessionId && x.UserId == userId)
                ?? throw new NotFoundException(StoreResponseConstants.SESSION_NOT_EXISTS);

            if (entity.EndTime != null)
            {
                throw new BadRequestException(StoreResponseConstants.SESSION_ALREADY_FINISHED);
            }

            entity.EndTime = DateTimeHelper.UtcNow();
            await context.SaveChangesAsync();

            return ResponseHelper.Create(entity.ToDto());
        }
    }

    public class UserAchievementService(SteamContext context) : IUserAchievementService
    {
        public GenericResponse<List<UserAchievementDto>> Get(Guid userId, FilterUserAchievementRequest model)
        {
            var queryable = context.UserAchievements
                .Include(x => x.Achievement)
                    .ThenInclude(x => x!.Game)
                .AsQueryable()
                .ApplyQuery(userId, model);

            var count = queryable.Count();
            var data = queryable
                .OrderByDescending(x => x.UnlockedAt)
                .Skip(model.Offset)
                .Take(model.Limit)
                .ToList()
                .Select(x => x.ToDto())
                .ToList();

            return ResponseHelper.Create(data, count: count);
        }

        public async Task<GenericResponse<UserAchievementDto>> Unlock(Guid userId, UnlockAchievementRequest model)
        {
            var achievement = await context.Achievements
                .Include(x => x.Game)
                .FirstOrDefaultAsync(x => x.AchievementId == model.AchievementId)
                ?? throw new NotFoundException(StoreResponseConstants.ACHIEVEMENT_NOT_EXISTS);

            var ownsGame = await context.Libraries.AnyAsync(x => x.UserId == userId && x.GameId == achievement.GameId);
            if (!ownsGame)
            {
                throw new BadRequestException(StoreResponseConstants.GAME_NOT_OWNED);
            }

            var alreadyUnlocked = await context.UserAchievements.AnyAsync(x => x.UserId == userId && x.AchievementId == model.AchievementId);
            if (alreadyUnlocked)
            {
                throw new BadRequestException(StoreResponseConstants.ACHIEVEMENT_ALREADY_UNLOCKED);
            }

            var entity = new UserAchievement
            {
                UserId = userId,
                AchievementId = model.AchievementId,
                UnlockedAt = DateTimeHelper.UtcNow()
            };

            await context.UserAchievements.AddAsync(entity);
            await context.SaveChangesAsync();

            var created = await context.UserAchievements
                .Include(x => x.Achievement)
                    .ThenInclude(x => x!.Game)
                .FirstAsync(x => x.UserAchievementId == entity.UserAchievementId);

            return ResponseHelper.Create(created.ToDto());
        }
    }
}
