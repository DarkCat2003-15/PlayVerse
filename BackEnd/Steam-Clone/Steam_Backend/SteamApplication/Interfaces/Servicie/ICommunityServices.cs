using SteamApplication.Models.Dtos;
using SteamApplication.Models.Request.Community;
using SteamApplication.Models.Response;

namespace SteamApplication.Interfaces.Servicie
{
    public interface IWishlistService
    {
        GenericResponse<List<WishlistItemDto>> Get(Guid userId);
        Task<GenericResponse<WishlistItemDto>> Add(Guid userId, CreateWishlistRequest model);
        Task<GenericResponse<bool>> Remove(Guid userId, Guid gameId);
    }

    public interface ILibraryService
    {
        GenericResponse<List<LibraryItemDto>> Get(Guid userId);
        Task<GenericResponse<LibraryItemDto>> Purchase(Guid userId, PurchaseGameRequest model);
    }

    public interface IReviewService
    {
        GenericResponse<List<ReviewDto>> Get(FilterReviewRequest model);
        Task<GenericResponse<ReviewDto>> Create(Guid userId, CreateReviewRequest model);
        Task<GenericResponse<ReviewDto>> Update(Guid userId, int reviewId, UpdateReviewRequest model);
        Task<GenericResponse<bool>> Delete(Guid userId, int reviewId);
        Task<GenericResponse<ReviewAnswerDto>> Answer(Guid userId, int reviewId, CreateReviewAnswerRequest model);
    }

    public interface IFriendService
    {
        GenericResponse<List<FriendDto>> Get(Guid userId);
        Task<GenericResponse<FriendDto>> Add(Guid userId, CreateFriendRequest model);
        Task<GenericResponse<bool>> Remove(Guid userId, Guid friendId);
    }

    public interface IGameSessionService
    {
        GenericResponse<List<GameSessionDto>> Get(Guid userId, FilterSessionRequest model);
        Task<GenericResponse<GameSessionDto>> Start(Guid userId, CreateSessionRequest model);
        Task<GenericResponse<GameSessionDto>> End(Guid userId, int sessionId);
    }

    public interface IUserAchievementService
    {
        GenericResponse<List<UserAchievementDto>> Get(Guid userId, FilterUserAchievementRequest model);
        Task<GenericResponse<UserAchievementDto>> Unlock(Guid userId, UnlockAchievementRequest model);
    }
}
