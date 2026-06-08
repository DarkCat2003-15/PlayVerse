using SteamShared.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SteamApplication.Models.Request.Auth.RecoverPassword
{
    public class RecoverPasswordSendOTPAuthRequest
    {
        [Required(ErrorMessage = ValidationConstants.Required)]
        [EmailAddress(ErrorMessage = ValidationConstants.EMAIL_ADDRESS)]
        [MaxLength(255, ErrorMessage = ValidationConstants.MaxLength)]
        [MinLength(10, ErrorMessage = ValidationConstants.MinLength)]
        [Description("El correo electrónico del usuario, para envíarle el código OTP")]
        public string Email { get; set; } = null!;
    }
}
