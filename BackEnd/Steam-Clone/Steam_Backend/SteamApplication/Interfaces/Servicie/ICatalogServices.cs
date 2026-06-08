using SteamApplication.Models.Dtos;
using SteamApplication.Models.Request.Catalog;
using SteamApplication.Models.Response;

namespace SteamApplication.Interfaces.Servicie
{
    public interface IDeveloperService
    {
        Task<GenericResponse<DeveloperDto>> Create(CreateDeveloperRequest model);
        GenericResponse<List<DeveloperDto>> Get(FilterDeveloperRequest model);
        Task<GenericResponse<DeveloperDto>> Get(Guid id);
        Task<GenericResponse<DeveloperDto>> Update(Guid id, UpdateDeveloperRequest model);
        Task<GenericResponse<bool>> Delete(Guid id);
    }

    public interface IPublisherService
    {
        Task<GenericResponse<PublisherDto>> Create(CreatePublisherRequest model);
        GenericResponse<List<PublisherDto>> Get(FilterPublisherRequest model);
        Task<GenericResponse<PublisherDto>> Get(Guid id);
        Task<GenericResponse<PublisherDto>> Update(Guid id, UpdatePublisherRequest model);
        Task<GenericResponse<bool>> Delete(Guid id);
    }

    public interface IGenreService
    {
        Task<GenericResponse<GenreDto>> Create(CreateGenreRequest model);
        GenericResponse<List<GenreDto>> Get(FilterGenreRequest model);
        Task<GenericResponse<GenreDto>> Get(int id);
        Task<GenericResponse<GenreDto>> Update(int id, UpdateGenreRequest model);
        Task<GenericResponse<bool>> Delete(int id);
    }

    public interface IGameService
    {
        Task<GenericResponse<GameDto>> Create(CreateGameRequest model);
        GenericResponse<List<GameCardDto>> Get(FilterGameRequest model);
        Task<GenericResponse<GameDto>> Get(Guid id);
        Task<GenericResponse<GameDto>> Update(Guid id, UpdateGameRequest model);
        Task<GenericResponse<bool>> Delete(Guid id);
        Task<GenericResponse<AchievementDto>> AddAchievement(Guid gameId, CreateAchievementRequest model);
        Task<GenericResponse<DlcDto>> AddDlc(Guid gameId, CreateDlcRequest model);
        Task<GenericResponse<OfferDto>> AddOffer(Guid gameId, CreateOfferRequest model);
    }
}
