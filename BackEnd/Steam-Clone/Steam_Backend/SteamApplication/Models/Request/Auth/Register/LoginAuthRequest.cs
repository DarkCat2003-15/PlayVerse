using SteamShared.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SteamApplication.Models.Request.Auth.Register
{
    public class LoginAuthRequest
    {
        [Required(ErrorMessage = ValidationConstants.Required)]
        [EmailAddress(ErrorMessage = ValidationConstants.EMAIL_ADDRESS)]
        [Description("Correo del Usuario")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = ValidationConstants.Required)]
        [MaxLength(255, ErrorMessage = ValidationConstants.MaxLength)]
        [MinLength(3, ErrorMessage = ValidationConstants.MinLength)]
        [Description("Contraseña del usuario")]
        public string Password { get; set; } = null!;

    }
}
