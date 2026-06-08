using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SteamApplication.Models.Request.Auth.Register
{
    public class RenewAuthRequest
    {
        [Required]
        [Description("Token que se usa para renovar la sesión. Este se consigue, al iniciar sesión en el aplicativo.")]
        public string RefreshToken { get; set; } = null!;
    }
}
