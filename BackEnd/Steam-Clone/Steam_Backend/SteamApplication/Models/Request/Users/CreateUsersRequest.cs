using SteamShared.Constants;
using System.ComponentModel.DataAnnotations;


namespace SteamApplication.Models.Request.Users
{
    public class CreateUsersRequest
    {
        /*[Required(ErrorMessage = ValidationConstants.Required)]
        [MinLength(10, ErrorMessage = ValidationConstants.MinLength)]
        [MaxLength(150, ErrorMessage = ValidationConstants.MaxLength)]
        public string FullName { get; set; } = null!; */
        [Required(ErrorMessage = ValidationConstants.Required)]
        [EmailAddress(ErrorMessage = ValidationConstants.EMAIL_ADDRESS)]
        [MaxLength(255, ErrorMessage = ValidationConstants.MaxLength)]
        [MinLength(10, ErrorMessage = ValidationConstants.MinLength)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = ValidationConstants.Required)]
        [MinLength(5, ErrorMessage = ValidationConstants.MinLength)]
        public string Username { get; set; } = null!;


        [Required(ErrorMessage = ValidationConstants.Required)]
        [MinLength(8, ErrorMessage = ValidationConstants.MinLength)]
        [MaxLength(20, ErrorMessage = ValidationConstants.MaxLength)]
        public string Password { get; set; } = null!;
    }
}
