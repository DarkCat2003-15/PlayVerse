using System.ComponentModel;

namespace SteamApplication.Models.Dtos
{
    public class UserDto
    {
        [Description("Identificador unico del usuario")]
        public Guid UserId { get; set; }

        [Description("Correo del usuario")]
        public string Email { get; set; } = string.Empty;

        [Description("Nombre de usuario")]
        public string Username { get; set; } = string.Empty;

        [Description("Fecha de creacion del usuario")]
        public DateTime CreatedAt { get; set; }

        [Description("Estado numerico del usuario. 1 = Activo, 0 = Desactivo")]
        public int StatusId { get; set; }

        [Description("Nombre del estado del usuario. Activo o Desactivo")]
        public string StatusName { get; set; } = string.Empty;

        [Description("Indica si el usuario esta activo")]
        public bool IsActive { get; set; }

        [Description("Roles asociados al usuario")]
        public List<string> Roles { get; set; } = [];
    }
}
