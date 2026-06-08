using System.ComponentModel;

namespace SteamApplication.Models.Dtos
{
    public class DeveloperDto
    {
        [Description("Identificador unico del desarrollador")]
        public Guid DeveloperId { get; set; }

        [Description("Nombre del desarrollador")]
        public string DeveloperName { get; set; } = string.Empty;

        [Description("Correo del desarrollador")]
        public string Email { get; set; } = string.Empty;

        [Description("Fecha de creacion")]
        public DateTime? CreatedAt { get; set; }

        [Description("Indica si el desarrollador fue eliminado logicamente")]
        public bool IsDeleted { get; set; }
    }

    public class PublisherDto
    {
        public Guid PublisherId { get; set; }
        public string PublisherName { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class GenreDto
    {
        public int GenreId { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class AchievementDto
    {
        public int AchievementId { get; set; }
        public Guid? GameId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class DlcDto
    {
        public int DlcId { get; set; }
        public Guid? GameId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime? AddedAt { get; set; }
    }

    public class OfferDto
    {
        public int OfferId { get; set; }
        public Guid? GameId { get; set; }
        public int Discount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class GameCardDto
    {
        public Guid GameId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public DateOnly? ReleaseDate { get; set; }
        public decimal BasePrice { get; set; }
        public decimal FinalPrice { get; set; }
        public bool HasActiveOffer { get; set; }
        public int DiscountPercentage { get; set; }
        public string DeveloperName { get; set; } = string.Empty;
        public string PublisherName { get; set; } = string.Empty;
        public List<string> Genres { get; set; } = [];
    }

    public class GameDto : GameCardDto
    {
        public Guid? DeveloperId { get; set; }
        public Guid? PublisherId { get; set; }
        public List<GenreDto> GenreItems { get; set; } = [];
        public List<AchievementDto> Achievements { get; set; } = [];
        public List<DlcDto> Dlcs { get; set; } = [];
        public List<OfferDto> Offers { get; set; } = [];
        public int ReviewCount { get; set; }
        public int RecommendedCount { get; set; }
        public decimal RecommendationRate { get; set; }
    }
}
