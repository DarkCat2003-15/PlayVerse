using SteamShared.Constants;
using System.ComponentModel.DataAnnotations;

namespace SteamApplication.Models.Request.Users
{
    public class UpdateUserRequest
    {


        [EmailAddress(ErrorMessage = ValidationConstants.EMAIL_ADDRESS)]
        [MaxLength(255, ErrorMessage = ValidationConstants.MaxLength)]
        [MinLength(10, ErrorMessage = ValidationConstants.MinLength)]
        public string? Email { get; set; } = null!;


        [Required(ErrorMessage = ValidationConstants.Required)]
        [MinLength(5, ErrorMessage = ValidationConstants.MinLength)]
        public string? Username { get; set; }

    }
}
