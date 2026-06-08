using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Steam.Web.Api.Helper;
using SteamApplication.Interfaces.Servicie;
using SteamApplication.Models.Dtos;
using SteamApplication.Models.Request.Roles;
using SteamApplication.Models.Response;
using SteamShared.Constants;

namespace Steam.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController(IRoleService service) : ControllerBase
    {
        [HttpGet]
        [Authorize(Policy = PermissionConstants.ROLES_READ)]
        [EndpointSummary("Listar roles del sistema")]
        public async Task<GenericResponse<List<RoleDto>>> GetRoles()
        {
            var srv = await service.GetRoles();
            return ResponseStatus.Ok(HttpContext, srv);
        }

        [HttpGet("permissions")]
        [Authorize(Policy = PermissionConstants.ROLES_READ)]
        [EndpointSummary("Listar permisos del sistema")]
        public async Task<GenericResponse<List<PermissionDto>>> GetPermissions()
        {
            var srv = await service.GetPermissions();
            return ResponseStatus.Ok(HttpContext, srv);
        }

        [HttpGet("me")]
        [EndpointSummary("Obtener roles del usuario autenticado")]
        public async Task<GenericResponse<List<RoleDto>>> MyRoles()
        {
            var srv = await service.GetMyRoles(CurrentUserHelper.GetRequiredUserId(User));
            return ResponseStatus.Ok(HttpContext, srv);
        }

        [HttpPost("assign")]
        [Authorize(Policy = PermissionConstants.ROLES_ASSIGN)]
        [EndpointSummary("Asignar rol a un usuario")]
        public async Task<GenericResponse<UserRoleDto>> AssignRole([FromBody] AssignRoleRequest model)
        {
            var srv = await service.AssignRole(model, CurrentUserHelper.GetRequiredUserId(User));
            return ResponseStatus.Created(HttpContext, srv);
        }

        [HttpDelete("users/{userId:guid}/roles/{roleId:guid}")]
        [Authorize(Policy = PermissionConstants.ROLES_ASSIGN)]
        [EndpointSummary("Quitar rol a un usuario")]
        public async Task<GenericResponse<bool>> RemoveRole(Guid userId, Guid roleId)
        {
            var srv = await service.RemoveRole(userId, roleId);
            return ResponseStatus.Ok(HttpContext, srv);
        }
    }
}
