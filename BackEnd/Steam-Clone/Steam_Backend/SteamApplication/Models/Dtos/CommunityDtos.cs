namespace SteamApplication.Models.Dtos
{
    public class WishlistItemDto
    {
        public int WishlistId { get; set; }
        public DateTime? AddedAt { get; set; }
        public GameCardDto Game { get; set; } = new();
    }

    public class LibraryItemDto
    {
        public int LibraryId { get; set; }
        public decimal PurchasePrice { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public GameCardDto Game { get; set; } = new();
    }

    public class ReviewAnswerDto
    {
        public int ReviewAnswerId { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class ReviewDto
    {
        public int ReviewId { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public Guid GameId { get; set; }
        public bool IsRecommended { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<ReviewAnswerDto> Answers { get; set; } = [];
    }

    public class FriendDto
    {
        public Guid FriendId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime? AddedAt { get; set; }
    }

    public class GameSessionDto
    {
        public int SessionId { get; set; }
        public Guid GameId { get; set; }
        public string GameTitle { get; set; } = string.Empty;
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public double DurationMinutes { get; set; }
    }

    public class UserAchievementDto
    {
        public int UserAchievementId { get; set; }
        public int AchievementId { get; set; }
        public string AchievementName { get; set; } = string.Empty;
        public string AchievementDescription { get; set; } = string.Empty;
        public Guid? GameId { get; set; }
        public string GameTitle { get; set; } = string.Empty;
        public DateTime? UnlockedAt { get; set; }
    }
}
