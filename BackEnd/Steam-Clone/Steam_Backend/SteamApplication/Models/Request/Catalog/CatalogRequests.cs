using SteamShared.Constants;
using System.ComponentModel.DataAnnotations;

namespace SteamApplication.Models.Request.Catalog
{
    public class FilterDeveloperRequest : BaseRequest
    {
        public string? Search { get; set; }
        public bool IncludeDeleted { get; set; }
    }

    public class CreateDeveloperRequest
    {
        [Required(ErrorMessage = ValidationConstants.Required)]
        [MaxLength(150, ErrorMessage = ValidationConstants.MaxLength)]
        public string DeveloperName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = ValidationConstants.EMAIL_ADDRESS)]
        [MaxLength(150, ErrorMessage = ValidationConstants.MaxLength)]
        public string? Email { get; set; }

        [MaxLength(255, ErrorMessage = ValidationConstants.MaxLength)]
        public string? Password { get; set; }
    }

    public class UpdateDeveloperRequest
    {
        [MaxLength(150, ErrorMessage = ValidationConstants.MaxLength)]
        public string? DeveloperName { get; set; }

        [EmailAddress(ErrorMessage = ValidationConstants.EMAIL_ADDRESS)]
        [MaxLength(150, ErrorMessage = ValidationConstants.MaxLength)]
        public string? Email { get; set; }

        [MaxLength(255, ErrorMessage = ValidationConstants.MaxLength)]
        public string? Password { get; set; }
    }

    public class FilterPublisherRequest : BaseRequest
    {
        public string? Search { get; set; }
        public bool IncludeDeleted { get; set; }
    }

    public class CreatePublisherRequest
    {
        [Required(ErrorMessage = ValidationConstants.Required)]
        [MaxLength(150, ErrorMessage = ValidationConstants.MaxLength)]
        public string PublisherName { get; set; } = string.Empty;
    }

    public class UpdatePublisherRequest
    {
        [MaxLength(150, ErrorMessage = ValidationConstants.MaxLength)]
        public string? PublisherName { get; set; }
    }

    public class FilterGenreRequest : BaseRequest
    {
        public string? Search { get; set; }
    }

    public class CreateGenreRequest
    {
        [Required(ErrorMessage = ValidationConstants.Required)]
        [MaxLength(100, ErrorMessage = ValidationConstants.MaxLength)]
        public string Name { get; set; } = string.Empty;
    }

    public class UpdateGenreRequest
    {
        [Required(ErrorMessage = ValidationConstants.Required)]
        [MaxLength(100, ErrorMessage = ValidationConstants.MaxLength)]
        public string Name { get; set; } = string.Empty;
    }

    public class FilterGameRequest : BaseRequest
    {
        public string? Search { get; set; }
        public Guid? DeveloperId { get; set; }
        public Guid? PublisherId { get; set; }
        public int? GenreId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool OnlyDiscounted { get; set; }
    }

    public class CreateGameRequest
    {
        [Required(ErrorMessage = ValidationConstants.Required)]
        [MaxLength(200, ErrorMessage = ValidationConstants.MaxLength)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000, ErrorMessage = ValidationConstants.MaxLength)]
        public string? Description { get; set; }

        [MaxLength(600, ErrorMessage = ValidationConstants.MaxLength)]
        public string? ImageUrl { get; set; }

        public DateOnly? ReleaseDate { get; set; }

        [Range(0, 999999)]
        public decimal Price { get; set; }

        public Guid? DeveloperId { get; set; }
        public Guid? PublisherId { get; set; }
        public List<int> GenreIds { get; set; } = [];
    }

    public class UpdateGameRequest
    {
        [MaxLength(200, ErrorMessage = ValidationConstants.MaxLength)]
        public string? Title { get; set; }

        [MaxLength(1000, ErrorMessage = ValidationConstants.MaxLength)]
        public string? Description { get; set; }

        [MaxLength(600, ErrorMessage = ValidationConstants.MaxLength)]
        public string? ImageUrl { get; set; }

        public DateOnly? ReleaseDate { get; set; }

        [Range(0, 999999)]
        public decimal? Price { get; set; }

        public Guid? DeveloperId { get; set; }
        public Guid? PublisherId { get; set; }
        public List<int>? GenreIds { get; set; }
    }

    public class CreateAchievementRequest
    {
        [Required(ErrorMessage = ValidationConstants.Required)]
        [MaxLength(200, ErrorMessage = ValidationConstants.MaxLength)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = ValidationConstants.MaxLength)]
        public string? Description { get; set; }
    }

    public class CreateDlcRequest
    {
        [Required(ErrorMessage = ValidationConstants.Required)]
        [MaxLength(200, ErrorMessage = ValidationConstants.MaxLength)]
        public string Name { get; set; } = string.Empty;

        [Range(0, 999999)]
        public decimal Price { get; set; }
    }

    public class CreateOfferRequest
    {
        [Range(1, 100)]
        public int Discount { get; set; }

        [Required(ErrorMessage = ValidationConstants.Required)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = ValidationConstants.Required)]
        public DateTime EndDate { get; set; }
    }
}
