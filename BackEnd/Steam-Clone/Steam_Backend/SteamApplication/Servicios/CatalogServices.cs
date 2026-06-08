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
    public class DeveloperService(SteamContext context) : IDeveloperService
    {
        public async Task<GenericResponse<DeveloperDto>> Create(CreateDeveloperRequest model)
        {
            var entity = new Developer
            {
                DeveloperName = model.DeveloperName,
                Email = model.Email,
                Password = string.IsNullOrWhiteSpace(model.Password) ? null : Hasher.HashPassword(model.Password),
                CreatedAt = DateTimeHelper.UtcNow()
            };

            await context.Developers.AddAsync(entity);
            await context.SaveChangesAsync();

            return ResponseHelper.Create(entity.ToDto());
        }

        public GenericResponse<List<DeveloperDto>> Get(FilterDeveloperRequest model)
        {
            var queryable = context.Developers.AsQueryable().ApplyQuery(model);
            var count = queryable.Count();
            var data = queryable
                .OrderBy(x => x.DeveloperName)
                .Skip(model.Offset)
                .Take(model.Limit)
                .ToList()
                .Select(x => x.ToDto())
                .ToList();

            return ResponseHelper.Create(data, count: count);
        }

        public async Task<GenericResponse<DeveloperDto>> Get(Guid id)
        {
            var entity = await context.Developers.FirstOrDefaultAsync(x => x.DeveloperId == id)
                ?? throw new NotFoundException(StoreResponseConstants.DEVELOPER_NOT_EXISTS);

            return ResponseHelper.Create(entity.ToDto());
        }

        public async Task<GenericResponse<DeveloperDto>> Update(Guid id, UpdateDeveloperRequest model)
        {
            var entity = await context.Developers.FirstOrDefaultAsync(x => x.DeveloperId == id)
                ?? throw new NotFoundException(StoreResponseConstants.DEVELOPER_NOT_EXISTS);

            if (!string.IsNullOrWhiteSpace(model.DeveloperName))
            {
                entity.DeveloperName = model.DeveloperName;
            }

            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                entity.Email = model.Email;
            }

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                entity.Password = Hasher.HashPassword(model.Password);
            }

            await context.SaveChangesAsync();
            return ResponseHelper.Create(entity.ToDto());
        }

        public async Task<GenericResponse<bool>> Delete(Guid id)
        {
            var entity = await context.Developers.FirstOrDefaultAsync(x => x.DeveloperId == id)
                ?? throw new NotFoundException(StoreResponseConstants.DEVELOPER_NOT_EXISTS);

            entity.DeletedAt = DateTimeHelper.UtcNow();
            await context.SaveChangesAsync();

            return ResponseHelper.Create(true);
        }
    }

    public class PublisherService(SteamContext context) : IPublisherService
    {
        public async Task<GenericResponse<PublisherDto>> Create(CreatePublisherRequest model)
        {
            var entity = new Publisher
            {
                PublisherName = model.PublisherName,
                CreatedAt = DateTimeHelper.UtcNow()
            };

            await context.Publishers.AddAsync(entity);
            await context.SaveChangesAsync();

            return ResponseHelper.Create(entity.ToDto());
        }

        public GenericResponse<List<PublisherDto>> Get(FilterPublisherRequest model)
        {
            var queryable = context.Publishers.AsQueryable().ApplyQuery(model);
            var count = queryable.Count();
            var data = queryable
                .OrderBy(x => x.PublisherName)
                .Skip(model.Offset)
                .Take(model.Limit)
                .ToList()
                .Select(x => x.ToDto())
                .ToList();

            return ResponseHelper.Create(data, count: count);
        }

        public async Task<GenericResponse<PublisherDto>> Get(Guid id)
        {
            var entity = await context.Publishers.FirstOrDefaultAsync(x => x.PublisherId == id)
                ?? throw new NotFoundException(StoreResponseConstants.PUBLISHER_NOT_EXISTS);

            return ResponseHelper.Create(entity.ToDto());
        }

        public async Task<GenericResponse<PublisherDto>> Update(Guid id, UpdatePublisherRequest model)
        {
            var entity = await context.Publishers.FirstOrDefaultAsync(x => x.PublisherId == id)
                ?? throw new NotFoundException(StoreResponseConstants.PUBLISHER_NOT_EXISTS);

            if (!string.IsNullOrWhiteSpace(model.PublisherName))
            {
                entity.PublisherName = model.PublisherName;
            }

            await context.SaveChangesAsync();
            return ResponseHelper.Create(entity.ToDto());
        }

        public async Task<GenericResponse<bool>> Delete(Guid id)
        {
            var entity = await context.Publishers.FirstOrDefaultAsync(x => x.PublisherId == id)
                ?? throw new NotFoundException(StoreResponseConstants.PUBLISHER_NOT_EXISTS);

            entity.DeletedAt = DateTimeHelper.UtcNow();
            await context.SaveChangesAsync();

            return ResponseHelper.Create(true);
        }
    }

    public class GenreService(SteamContext context) : IGenreService
    {
        public async Task<GenericResponse<GenreDto>> Create(CreateGenreRequest model)
        {
            var entity = new Genre
            {
                Name = model.Name
            };

            await context.Genres.AddAsync(entity);
            await context.SaveChangesAsync();

            return ResponseHelper.Create(entity.ToDto());
        }

        public GenericResponse<List<GenreDto>> Get(FilterGenreRequest model)
        {
            var queryable = context.Genres.AsQueryable().ApplyQuery(model);
            var count = queryable.Count();
            var data = queryable
                .OrderBy(x => x.Name)
                .Skip(model.Offset)
                .Take(model.Limit)
                .ToList()
                .Select(x => x.ToDto())
                .ToList();

            return ResponseHelper.Create(data, count: count);
        }

        public async Task<GenericResponse<GenreDto>> Get(int id)
        {
            var entity = await context.Genres.FirstOrDefaultAsync(x => x.GenreId == id)
                ?? throw new NotFoundException(StoreResponseConstants.GENRE_NOT_EXISTS);

            return ResponseHelper.Create(entity.ToDto());
        }

        public async Task<GenericResponse<GenreDto>> Update(int id, UpdateGenreRequest model)
        {
            var entity = await context.Genres.FirstOrDefaultAsync(x => x.GenreId == id)
                ?? throw new NotFoundException(StoreResponseConstants.GENRE_NOT_EXISTS);

            entity.Name = model.Name;
            await context.SaveChangesAsync();

            return ResponseHelper.Create(entity.ToDto());
        }

        public async Task<GenericResponse<bool>> Delete(int id)
        {
            var entity = await context.Genres.FirstOrDefaultAsync(x => x.GenreId == id)
                ?? throw new NotFoundException(StoreResponseConstants.GENRE_NOT_EXISTS);

            var inUse = await context.GameGenres.AnyAsync(x => x.GenreId == id);
            if (inUse)
            {
                throw new BadRequestException(StoreResponseConstants.GENRE_IN_USE);
            }

            context.Genres.Remove(entity);
            await context.SaveChangesAsync();

            return ResponseHelper.Create(true);
        }
    }
}
