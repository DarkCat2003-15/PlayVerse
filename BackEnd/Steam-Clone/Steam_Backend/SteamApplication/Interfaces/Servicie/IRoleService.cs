using SteamApplication.Models.Dtos;
using SteamApplication.Models.Request.Roles;
using SteamApplication.Models.Response;

namespace SteamApplication.Interfaces.Servicie
{
    public interface IRoleService
    {
        Task<GenericResponse<List<RoleDto>>> GetRoles();
        Task<GenericResponse<List<PermissionDto>>> GetPermissions();
        Task<GenericResponse<List<RoleDto>>> GetMyRoles(Guid userId);
        Task<GenericResponse<UserRoleDto>> AssignRole(AssignRoleRequest model, Guid assignedBy);
        Task<GenericResponse<bool>> RemoveRole(Guid userId, Guid roleId);
    }
}
