using SteamApplication.Models.Dtos;
using SteamDomain.Database.SqlServer.Entities;

namespace SteamApplication.Helpers
{
    public static class StoreMapper
    {
        public static DeveloperDto ToDto(this Developer developer)
        {
            return new DeveloperDto
            {
                DeveloperId = developer.DeveloperId,
                DeveloperName = developer.DeveloperName ?? string.Empty,
                Email = developer.Email ?? string.Empty,
                CreatedAt = developer.CreatedAt,
                IsDeleted = developer.DeletedAt.HasValue
            };
        }

        public static PublisherDto ToDto(this Publisher publisher)
        {
            return new PublisherDto
            {
                PublisherId = publisher.PublisherId,
                PublisherName = publisher.PublisherName ?? string.Empty,
                CreatedAt = publisher.CreatedAt,
                IsDeleted = publisher.DeletedAt.HasValue
            };
        }

        public static GenreDto ToDto(this Genre genre)
        {
            return new GenreDto
            {
                GenreId = genre.GenreId,
                Name = genre.Name ?? string.Empty
            };
        }

        public static AchievementDto ToDto(this Achievement achievement)
        {
            return new AchievementDto
            {
                AchievementId = achievement.AchievementId,
                GameId = achievement.GameId,
                Name = achievement.Name ?? string.Empty,
                Description = achievement.Description ?? string.Empty
            };
        }

        public static DlcDto ToDto(this Dlc dlc)
        {
            return new DlcDto
            {
                DlcId = dlc.Dlcid,
                GameId = dlc.GameId,
                Name = dlc.Dlcname ?? string.Empty,
                Price = dlc.Price ?? 0m,
                AddedAt = dlc.AddedAt
            };
        }

        public static OfferDto ToDto(this Offer offer, DateTime now)
        {
            return new OfferDto
            {
                OfferId = offer.OfferId,
                GameId = offer.GameId,
                Discount = offer.Discount ?? 0,
                StartDate = offer.StartDate,
                EndDate = offer.EndDate,
                IsActive = offer.StartDate.HasValue && offer.EndDate.HasValue && offer.StartDate <= now && offer.EndDate >= now
            };
        }

        public static GameCardDto ToCardDto(this Game game, DateTime now)
        {
            var activeOffer = GamePriceHelper.GetActiveOffer(game, now);

            return new GameCardDto
            {
                GameId = game.GameId,
                Title = game.Title ?? string.Empty,
                Description = game.Description ?? string.Empty,
                ImageUrl = game.ImageUrl ?? string.Empty,
                ReleaseDate = game.ReleaseDate,
                BasePrice = game.Price ?? 0m,
                FinalPrice = GamePriceHelper.ResolveCurrentPrice(game, now),
                HasActiveOffer = activeOffer != null,
                DiscountPercentage = activeOffer?.Discount ?? 0,
                DeveloperName = game.Developer?.DeveloperName ?? string.Empty,
                PublisherName = game.Publisher?.PublisherName ?? string.Empty,
                Genres = game.GameGenres
                    .Where(x => x.Genre != null)
                    .Select(x => x.Genre!.Name ?? string.Empty)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct()
                    .ToList()
            };
        }

        public static GameDto ToDto(this Game game, DateTime now)
        {
            var card = game.ToCardDto(now);
            var reviews = game.Reviews.Where(review => review.DeletedAt == null).ToList();
            var reviewCount = reviews.Count;
            var recommendedCount = reviews.Count(review => review.IsRecommended == true);

            return new GameDto
            {
                GameId = card.GameId,
                Title = card.Title,
                Description = card.Description,
                ImageUrl = card.ImageUrl,
                ReleaseDate = card.ReleaseDate,
                BasePrice = card.BasePrice,
                FinalPrice = card.FinalPrice,
                HasActiveOffer = card.HasActiveOffer,
                DiscountPercentage = card.DiscountPercentage,
                DeveloperId = game.DeveloperId,
                DeveloperName = card.DeveloperName,
                PublisherId = game.PublisherId,
                PublisherName = card.PublisherName,
                Genres = card.Genres,
                GenreItems = game.GameGenres
                    .Where(x => x.Genre != null)
                    .Select(x => x.Genre!.ToDto())
                    .DistinctBy(x => x.GenreId)
                    .ToList(),
                Achievements = game.Achievements.Select(x => x.ToDto()).ToList(),
                Dlcs = game.Dlcs.Select(x => x.ToDto()).ToList(),
                Offers = game.Offers.Select(x => x.ToDto(now)).OrderByDescending(x => x.StartDate).ToList(),
                ReviewCount = reviewCount,
                RecommendedCount = recommendedCount,
                RecommendationRate = reviewCount == 0
                    ? 0m
                    : decimal.Round((decimal)recommendedCount * 100m / reviewCount, 2, MidpointRounding.AwayFromZero)
            };
        }

        public static LibraryItemDto ToDto(this Library library, DateTime now)
        {
            return new LibraryItemDto
            {
                LibraryId = library.LibraryId,
                PurchasePrice = library.PurchasePrice ?? 0m,
                PurchaseDate = library.PurchaseDate,
                Game = library.Game?.ToCardDto(now) ?? new GameCardDto()
            };
        }

        public static WishlistItemDto ToDto(this Wishlist wishlist, DateTime now)
        {
            return new WishlistItemDto
            {
                WishlistId = wishlist.WishlistId,
                AddedAt = wishlist.AddedAt,
                Game = wishlist.Game?.ToCardDto(now) ?? new GameCardDto()
            };
        }

        public static ReviewAnswerDto ToDto(this ReviewAnswer reviewAnswer)
        {
            return new ReviewAnswerDto
            {
                ReviewAnswerId = reviewAnswer.ReviewAnswersId,
                UserId = reviewAnswer.UserId,
                Username = reviewAnswer.User?.UserName ?? string.Empty,
                Comment = reviewAnswer.Comment ?? string.Empty,
                CreatedAt = reviewAnswer.CreatedAt,
                UpdatedAt = reviewAnswer.UpdatedAt
            };
        }

        public static ReviewDto ToDto(this Review review)
        {
            return new ReviewDto
            {
                ReviewId = review.ReviewId,
                UserId = review.UserId ?? Guid.Empty,
                Username = review.User?.UserName ?? string.Empty,
                GameId = review.GameId ?? Guid.Empty,
                IsRecommended = review.IsRecommended ?? false,
                Comment = review.Comment ?? string.Empty,
                CreatedAt = review.CreatedAt,
                UpdatedAt = review.UpdatedAt,
                Answers = review.ReviewAnswers
                    .Where(x => x.DeletedAt == null)
                    .OrderBy(x => x.CreatedAt)
                    .Select(x => x.ToDto())
                    .ToList()
            };
        }

        public static FriendDto ToDto(this Friend friend)
        {
            return new FriendDto
            {
                FriendId = friend.FriendId,
                Username = friend.FriendNavigation?.UserName ?? string.Empty,
                Email = friend.FriendNavigation?.Email ?? string.Empty,
                AddedAt = friend.AddedAt
            };
        }

        public static GameSessionDto ToDto(this Session session)
        {
            var endTime = session.EndTime ?? DateTime.UtcNow;

            return new GameSessionDto
            {
                SessionId = session.SessionId,
                GameId = session.GameId ?? Guid.Empty,
                GameTitle = session.Game?.Title ?? string.Empty,
                StartTime = session.StartTime,
                EndTime = session.EndTime,
                DurationMinutes = session.StartTime.HasValue
                    ? Math.Round((endTime - session.StartTime.Value).TotalMinutes, 2)
                    : 0d
            };
        }

        public static UserAchievementDto ToDto(this UserAchievement userAchievement)
        {
            return new UserAchievementDto
            {
                UserAchievementId = userAchievement.UserAchievementId,
                AchievementId = userAchievement.AchievementId ?? 0,
                AchievementName = userAchievement.Achievement?.Name ?? string.Empty,
                AchievementDescription = userAchievement.Achievement?.Description ?? string.Empty,
                GameId = userAchievement.Achievement?.GameId,
                GameTitle = userAchievement.Achievement?.Game?.Title ?? string.Empty,
                UnlockedAt = userAchievement.UnlockedAt
            };
        }
    }
}
