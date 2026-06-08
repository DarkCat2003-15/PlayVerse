using SteamShared.Constants;
using System.ComponentModel.DataAnnotations;

namespace SteamApplication.Models.Request.Community
{
    public class CreateWishlistRequest
    {
        [Required(ErrorMessage = ValidationConstants.Required)]
        public Guid GameId { get; set; }
    }

    public class PurchaseGameRequest
    {
        [Required(ErrorMessage = ValidationConstants.Required)]
        public Guid GameId { get; set; }
    }

    public class CreateReviewRequest
    {
        [Required(ErrorMessage = ValidationConstants.Required)]
        public Guid GameId { get; set; }

        public bool IsRecommended { get; set; }

        [MaxLength(1000, ErrorMessage = ValidationConstants.MaxLength)]
        public string? Comment { get; set; }
    }

    public class UpdateReviewRequest
    {
        public bool IsRecommended { get; set; }

        [MaxLength(1000, ErrorMessage = ValidationConstants.MaxLength)]
        public string? Comment { get; set; }
    }

    public class FilterReviewRequest : BaseRequest
    {
        public Guid? GameId { get; set; }
        public Guid? UserId { get; set; }
        public bool? IsRecommended { get; set; }
    }

    public class CreateReviewAnswerRequest
    {
        [Required(ErrorMessage = ValidationConstants.Required)]
        [MaxLength(1000, ErrorMessage = ValidationConstants.MaxLength)]
        public string Comment { get; set; } = string.Empty;
    }

    public class CreateFriendRequest
    {
        [Required(ErrorMessage = ValidationConstants.Required)]
        public Guid FriendId { get; set; }
    }

    public class CreateSessionRequest
    {
        [Required(ErrorMessage = ValidationConstants.Required)]
        public Guid GameId { get; set; }
    }

    public class FilterSessionRequest : BaseRequest
    {
        public Guid? GameId { get; set; }
        public bool OnlyActive { get; set; }
    }

    public class UnlockAchievementRequest
    {
        [Required(ErrorMessage = ValidationConstants.Required)]
        public int AchievementId { get; set; }
    }

    public class FilterUserAchievementRequest : BaseRequest
    {
        public Guid? GameId { get; set; }
    }
}
