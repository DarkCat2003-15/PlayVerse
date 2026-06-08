using SteamApplication.Models.Request.Users;
using SteamDomain.Database.SqlServer.Entities;

namespace SteamApplication.Queries
{
    public static class UserFilterQuery
    {
        public static IQueryable<User> ApplyQuery(this IQueryable<User> queryable, FilterUserRequest model)
        {
            // Filtrado de nombre
            if (!string.IsNullOrWhiteSpace(model.Username))
            {
                queryable = queryable.Where(x => x.UserName.Contains(model.Username ?? ""));
            }

            return queryable;
        }
    }
}
