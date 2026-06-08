using Microsoft.EntityFrameworkCore;
using SteamApplication.Helpers;
using SteamApplication.Interfaces.Servicie;
using SteamApplication.Models.Dtos;
using SteamApplication.Models.Request.Catalog;
using SteamApplication.Models.Response;
using SteamApplication.Queries;
using SteamDomain.Database.SqlServer.Context;
using SteamDomain.Database.SqlServer.Entities;
using SteamDomain.Exceptions;
using SteamShared.Constants;
using SteamShared.Helpers;

namespace SteamApplication.Servicios
{
    public class GameService(SteamContext context) : IGameService
    {
        public async Task<GenericResponse<GameDto>> Create(CreateGameRequest model)
        {
            await EnsureDeveloperAndPublisher(model.DeveloperId, model.PublisherId);
            var genres = await GetGenres(model.GenreIds);

            var entity = new Game
            {
                Title = model.Title,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                ReleaseDate = model.ReleaseDate,
                Price = model.Price,
                DeveloperId = model.DeveloperId,
                PublisherId = model.PublisherId,
                GameGenres = genres.Select(x => new GameGenre { GenreId = x.GenreId }).ToList()
            };

            await context.Games.AddAsync(entity);
            await context.SaveChangesAsync();

            var created = await GetGameEntity(entity.GameId);
            return ResponseHelper.Create(created.ToDto(DateTimeHelper.UtcNow()));
        }

        public GenericResponse<List<GameCardDto>> Get(FilterGameRequest model)
        {
            var now = DateTimeHelper.UtcNow();
            var queryable = GetGameQueryable().ApplyQuery(model);
            var count = queryable.Count();
            var data = queryable
                .OrderBy(x => x.Title)
                .Skip(model.Offset)
                .Take(model.Limit)
                .ToList()
                .Select(x => x.ToCardDto(now))
                .ToList();

            return ResponseHelper.Create(data, count: count);
        }

        public async Task<GenericResponse<GameDto>> Get(Guid id)
        {
            var entity = await GetGameEntity(id);
            return ResponseHelper.Create(entity.ToDto(DateTimeHelper.UtcNow()));
        }

        public async Task<GenericResponse<GameDto>> Update(Guid id, UpdateGameRequest model)
        {
            var entity = await context.Games
                .Include(x => x.GameGenres)
                .FirstOrDefaultAsync(x => x.GameId == id && x.DeletedAt == null)
                ?? throw new NotFoundException(StoreResponseConstants.GAME_NOT_EXISTS);

            await EnsureDeveloperAndPublisher(model.DeveloperId, model.PublisherId);

            if (!string.IsNullOrWhiteSpace(model.Title))
            {
                entity.Title = model.Title;
            }

            if (model.Description != null)
            {
                entity.Description = model.Description;
            }

            if (model.ImageUrl != null)
            {
                entity.ImageUrl = model.ImageUrl;
            }

            if (model.ReleaseDate.HasValue)
            {
                entity.ReleaseDate = model.ReleaseDate;
            }

            if (model.Price.HasValue)
            {
                entity.Price = model.Price.Value;
            }

            if (model.DeveloperId.HasValue || model.DeveloperId == null)
            {
                entity.DeveloperId = model.DeveloperId;
            }

            if (model.PublisherId.HasValue || model.PublisherId == null)
            {
                entity.PublisherId = model.PublisherId;
            }

            if (model.GenreIds != null)
            {
                var genres = await GetGenres(model.GenreIds);
                context.GameGenres.RemoveRange(entity.GameGenres);
                entity.GameGenres = genres.Select(x => new GameGenre
                {
                    GameId = entity.GameId,
                    GenreId = x.GenreId
                }).ToList();
            }

            await context.SaveChangesAsync();

            var updated = await GetGameEntity(id);
            return ResponseHelper.Create(updated.ToDto(DateTimeHelper.UtcNow()));
        }

        public async Task<GenericResponse<bool>> Delete(Guid id)
        {
            var entity = await context.Games.FirstOrDefaultAsync(x => x.GameId == id && x.DeletedAt == null)
                ?? throw new NotFoundException(StoreResponseConstants.GAME_NOT_EXISTS);

            entity.DeletedAt = DateTimeHelper.UtcNow();
            await context.SaveChangesAsync();

            return ResponseHelper.Create(true);
        }

        public async Task<GenericResponse<AchievementDto>> AddAchievement(Guid gameId, CreateAchievementRequest model)
        {
            _ = await EnsureGameExists(gameId);

            var entity = new Achievement
            {
                GameId = gameId,
                Name = model.Name,
                Description = model.Description
            };

            await context.Achievements.AddAsync(entity);
            await context.SaveChangesAsync();

            return ResponseHelper.Create(entity.ToDto());
        }

        public async Task<GenericResponse<DlcDto>> AddDlc(Guid gameId, CreateDlcRequest model)
        {
            _ = await EnsureGameExists(gameId);

            var entity = new Dlc
            {
                GameId = gameId,
                Dlcname = model.Name,
                Price = model.Price,
                AddedAt = DateTimeHelper.UtcNow()
            };

            await context.Dlcs.AddAsync(entity);
            await context.SaveChangesAsync();

            return ResponseHelper.Create(entity.ToDto());
        }

        public async Task<GenericResponse<OfferDto>> AddOffer(Guid gameId, CreateOfferRequest model)
        {
            _ = await EnsureGameExists(gameId);

            if (model.StartDate >= model.EndDate)
            {
                throw new BadRequestException(StoreResponseConstants.OFFER_INVALID_RANGE);
            }

            var entity = new Offer
            {
                GameId = gameId,
                Discount = model.Discount,
                StartDate = model.StartDate,
                EndDate = model.EndDate
            };

            await context.Offers.AddAsync(entity);
            await context.SaveChangesAsync();

            return ResponseHelper.Create(entity.ToDto(DateTimeHelper.UtcNow()));
        }

        private IQueryable<Game> GetGameQueryable()
        {
            return context.Games
                .Include(x => x.Developer)
                .Include(x => x.Publisher)
                .Include(x => x.GameGenres)
                    .ThenInclude(x => x.Genre)
                .Include(x => x.Achievements)
                .Include(x => x.Dlcs)
                .Include(x => x.Offers)
                .Include(x => x.Reviews);
        }

        private async Task<Game> GetGameEntity(Guid id)
        {
            return await GetGameQueryable()
                .FirstOrDefaultAsync(x => x.GameId == id && x.DeletedAt == null)
                ?? throw new NotFoundException(StoreResponseConstants.GAME_NOT_EXISTS);
        }

        private async Task<Game> EnsureGameExists(Guid id)
        {
            return await context.Games.FirstOrDefaultAsync(x => x.GameId == id && x.DeletedAt == null)
                ?? throw new NotFoundException(StoreResponseConstants.GAME_NOT_EXISTS);
        }

        private async Task EnsureDeveloperAndPublisher(Guid? developerId, Guid? publisherId)
        {
            if (developerId.HasValue)
            {
                var exists = await context.Developers.AnyAsync(x => x.DeveloperId == developerId && x.DeletedAt == null);
                if (!exists)
                {
                    throw new NotFoundException(StoreResponseConstants.DEVELOPER_NOT_EXISTS);
                }
            }

            if (publisherId.HasValue)
            {
                var exists = await context.Publishers.AnyAsync(x => x.PublisherId == publisherId && x.DeletedAt == null);
                if (!exists)
                {
                    throw new NotFoundException(StoreResponseConstants.PUBLISHER_NOT_EXISTS);
                }
            }
        }

        private async Task<List<Genre>> GetGenres(List<int> genreIds)
        {
            if (genreIds.Count == 0)
            {
                return [];
            }

            var distinctIds = genreIds.Distinct().ToList();
            var genres = await context.Genres.Where(x => distinctIds.Contains(x.GenreId)).ToListAsync();

            if (genres.Count != distinctIds.Count)
            {
                throw new NotFoundException(StoreResponseConstants.GENRE_NOT_EXISTS);
            }

            return genres;
        }
    }
}
