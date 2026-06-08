using System.ComponentModel;

namespace SteamApplication.Models.Responses.Auth
{
    public class LoginAuthResponse
    {
        [Description("JWT del usuario autenticado")]
        public required string Token { get; set; }

        [Description("Refresh token para renovar la sesion")]
        public required string RefreshToken { get; set; }

        [Description("Identificador unico del usuario")]
        public required Guid UserId { get; set; }

        [Description("Correo del usuario autenticado")]
        public required string Email { get; set; }

        [Description("Nombre de usuario autenticado")]
        public required string Username { get; set; }

        [Description("Estado numerico del usuario. 1 = Activo, 0 = Desactivo")]
        public required int StatusId { get; set; }

        [Description("Nombre del estado del usuario. Activo o Desactivo")]
        public required string StatusName { get; set; }

        [Description("Indica si el usuario esta activo")]
        public required bool IsActive { get; set; }

        [Description("Roles activos del usuario")]
        public List<string> Roles { get; set; } = [];

        [Description("Permisos activos del usuario")]
        public List<string> Permissions { get; set; } = [];
    }
}
