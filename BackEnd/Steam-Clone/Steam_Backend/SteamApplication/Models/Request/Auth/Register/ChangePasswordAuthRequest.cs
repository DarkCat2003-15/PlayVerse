using SteamShared.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SteamApplication.Models.Request.Auth.Register
{
    public class ChangePasswordAuthRequest
    {
        [Required(ErrorMessage = ValidationConstants.Required)]
        [MaxLength(255, ErrorMessage = ValidationConstants.MaxLength)]
        [MinLength(8, ErrorMessage = ValidationConstants.MinLength)]
        [Description("La contraseña actual del usuario")]
        public string CurrentPassword { get; set; } = null!;

        [Required(ErrorMessage = ValidationConstants.Required)]
        [MaxLength(255, ErrorMessage = ValidationConstants.MaxLength)]
        [MinLength(8, ErrorMessage = ValidationConstants.MinLength)]
        [Description("La contraseña nueva del usuario")]
        public string NewPassword { get; set; } = null!;



    }
}
