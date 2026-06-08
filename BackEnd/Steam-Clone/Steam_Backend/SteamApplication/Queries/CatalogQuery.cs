using SteamApplication.Models.Request.Catalog;
using SteamDomain.Database.SqlServer.Entities;

namespace SteamApplication.Queries
{
    public static class CatalogQuery
    {
        public static IQueryable<Developer> ApplyQuery(this IQueryable<Developer> queryable, FilterDeveloperRequest model)
        {
            if (!model.IncludeDeleted)
            {
                queryable = queryable.Where(x => x.DeletedAt == null);
            }

            if (!string.IsNullOrWhiteSpace(model.Search))
            {
                queryable = queryable.Where(x =>
                    (x.DeveloperName ?? string.Empty).Contains(model.Search) ||
                    (x.Email ?? string.Empty).Contains(model.Search));
            }

            return queryable;
        }

        public static IQueryable<Publisher> ApplyQuery(this IQueryable<Publisher> queryable, FilterPublisherRequest model)
        {
            if (!model.IncludeDeleted)
            {
                queryable = queryable.Where(x => x.DeletedAt == null);
            }

            if (!string.IsNullOrWhiteSpace(model.Search))
            {
                queryable = queryable.Where(x => (x.PublisherName ?? string.Empty).Contains(model.Search));
            }

            return queryable;
        }

        public static IQueryable<Genre> ApplyQuery(this IQueryable<Genre> queryable, FilterGenreRequest model)
        {
            if (!string.IsNullOrWhiteSpace(model.Search))
            {
                queryable = queryable.Where(x => (x.Name ?? string.Empty).Contains(model.Search));
            }

            return queryable;
        }

        public static IQueryable<Game> ApplyQuery(this IQueryable<Game> queryable, FilterGameRequest model)
        {
            queryable = queryable.Where(x => x.DeletedAt == null);

            if (!string.IsNullOrWhiteSpace(model.Search))
            {
                queryable = queryable.Where(x =>
                    (x.Title ?? string.Empty).Contains(model.Search) ||
                    (x.Description ?? string.Empty).Contains(model.Search));
            }

            if (model.DeveloperId.HasValue)
            {
                queryable = queryable.Where(x => x.DeveloperId == model.DeveloperId);
            }

            if (model.PublisherId.HasValue)
            {
                queryable = queryable.Where(x => x.PublisherId == model.PublisherId);
            }

            if (model.GenreId.HasValue)
            {
                queryable = queryable.Where(x => x.GameGenres.Any(y => y.GenreId == model.GenreId));
            }

            if (model.MinPrice.HasValue)
            {
                queryable = queryable.Where(x => (x.Price ?? 0m) >= model.MinPrice.Value);
            }

            if (model.MaxPrice.HasValue)
            {
                queryable = queryable.Where(x => (x.Price ?? 0m) <= model.MaxPrice.Value);
            }

            if (model.OnlyDiscounted)
            {
                var now = DateTime.UtcNow;
                queryable = queryable.Where(x => x.Offers.Any(offer =>
                    offer.StartDate.HasValue &&
                    offer.EndDate.HasValue &&
                    offer.StartDate <= now &&
                    offer.EndDate >= now &&
                    (offer.Discount ?? 0) > 0));
            }

            return queryable;
        }
    }
}
