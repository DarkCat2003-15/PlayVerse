using SteamShared.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SteamApplication.Models.Request.Auth.Register
{
    public class RegisterInitAuthRequest
    {
        [Required(ErrorMessage = ValidationConstants.Required)]
        [EmailAddress(ErrorMessage = ValidationConstants.EMAIL_ADDRESS)]
        [MaxLength(255, ErrorMessage = ValidationConstants.MaxLength)]
        [MinLength(10, ErrorMessage = ValidationConstants.MinLength)]
        [Description("Correo del usuario que desea registrarse")]
        public string Email { get; set; } = null!;
    }
}
