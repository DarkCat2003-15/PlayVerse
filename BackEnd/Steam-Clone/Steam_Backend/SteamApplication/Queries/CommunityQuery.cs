using SteamApplication.Models.Request.Community;
using SteamDomain.Database.SqlServer.Entities;

namespace SteamApplication.Queries
{
    public static class CommunityQuery
    {
        public static IQueryable<Review> ApplyQuery(this IQueryable<Review> queryable, FilterReviewRequest model)
        {
            queryable = queryable.Where(x => x.DeletedAt == null);

            if (model.GameId.HasValue)
            {
                queryable = queryable.Where(x => x.GameId == model.GameId);
            }

            if (model.UserId.HasValue)
            {
                queryable = queryable.Where(x => x.UserId == model.UserId);
            }

            if (model.IsRecommended.HasValue)
            {
                queryable = queryable.Where(x => x.IsRecommended == model.IsRecommended);
            }

            return queryable;
        }

        public static IQueryable<Session> ApplyQuery(this IQueryable<Session> queryable, Guid userId, FilterSessionRequest model)
        {
            queryable = queryable.Where(x => x.UserId == userId);

            if (model.GameId.HasValue)
            {
                queryable = queryable.Where(x => x.GameId == model.GameId);
            }

            if (model.OnlyActive)
            {
                queryable = queryable.Where(x => x.EndTime == null);
            }

            return queryable;
        }

        public static IQueryable<UserAchievement> ApplyQuery(this IQueryable<UserAchievement> queryable, Guid userId, FilterUserAchievementRequest model)
        {
            queryable = queryable.Where(x => x.UserId == userId);

            if (model.GameId.HasValue)
            {
                queryable = queryable.Where(x => x.Achievement != null && x.Achievement.GameId == model.GameId);
            }

            return queryable;
        }
    }
}
