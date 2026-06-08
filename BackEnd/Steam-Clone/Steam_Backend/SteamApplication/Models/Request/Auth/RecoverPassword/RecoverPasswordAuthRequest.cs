using SteamShared.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SteamApplication.Models.Request.Auth.RecoverPassword
{
    public class RecoverPasswordAuthRequest
    {
        [Required(ErrorMessage = ValidationConstants.Required)]
        [MaxLength(255, ErrorMessage = ValidationConstants.MaxLength)]
        [MinLength(8, ErrorMessage = ValidationConstants.MinLength)]
        [Description("La nueva contraseña que quiere establecer el usuario")]
        public string NewPassword { get; set; } = null!;
    }
}
