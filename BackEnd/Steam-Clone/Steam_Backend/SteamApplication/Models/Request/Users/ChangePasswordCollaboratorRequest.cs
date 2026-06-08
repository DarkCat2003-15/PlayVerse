using SteamShared.Constants;
using System.ComponentModel.DataAnnotations;

namespace SteamApplication.Models.Request.Users
{
    public class ChangePasswordCollaboratorRequest
    {
        [Required(ErrorMessage = ValidationConstants.Required)]
        public string CurrentPassword { get; set; } = null!;
        [Required(ErrorMessage = ValidationConstants.Required)]
        public string NewPassword { get; set; } = null!;
    }
}
