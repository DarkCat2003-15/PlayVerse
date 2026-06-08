using SteamShared.Constants;
using System.ComponentModel.DataAnnotations;

namespace SteamApplication.Models.Request.Roles
{
    public class AssignRoleRequest
    {
        [Required(ErrorMessage = ValidationConstants.Required)]
        public Guid UserId { get; set; }

        public Guid? RoleId { get; set; }

        [MaxLength(100, ErrorMessage = ValidationConstants.MaxLength)]
        public string? RoleName { get; set; }
    }
}
