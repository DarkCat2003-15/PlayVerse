using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Steam.Web.Api.Helper;
using SteamApplication.Interfaces.Servicie;
using SteamApplication.Models.Dtos;
using SteamApplication.Models.Request.Users;
using SteamApplication.Models.Response;
using SteamShared.Constants;

namespace Steam.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        // ✅ CREAR USUARIO SIN TOKEN
        [HttpPost]
        [AllowAnonymous]
        [EndpointSummary("Crear usuario")]
        [EndpointDescription("Crea un usuario con StatusId = 1 (Activo).")]
        [ProducesResponseType(typeof(GenericResponse<UserDto>), StatusCodes.Status201Created)]
        public async Task<GenericResponse<UserDto>> Create([FromBody] CreateUsersRequest model)
        {
            var srv = await userService.Create(model);
            return ResponseStatus.Created(HttpContext, srv);
        }

        // ✅ OBTENER TODOS (PROTEGIDO)
        [HttpGet]
        [Authorize(Policy = PermissionConstants.USERS_READ)]
        [ProducesResponseType(typeof(GenericResponse<List<UserDto>>), StatusCodes.Status200OK)]
        public async Task<GenericResponse<List<UserDto>>> GetAll([FromQuery] FilterUserRequest model)
        {
            var srv = userService.Get(model);
            return ResponseStatus.Ok(HttpContext, srv);
        }

        // ✅ OBTENER POR ID
        [HttpGet("{id:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(GenericResponse<UserDto>), StatusCodes.Status200OK)]
        public async Task<GenericResponse<UserDto>> GetById(Guid id)
        {
            var currentUserId = GetUserId();
            if (id != currentUserId && !CurrentUserHelper.HasPermission(User, PermissionConstants.USERS_READ))
            {
                throw new UnauthorizedAccessException(ResponseConstants.AUTH_CLAIM_USER_NOT_FOUND);
            }

            var srv = await userService.Get(id);
            return ResponseStatus.Ok(HttpContext, srv);
        }

        // ✅ USUARIO ACTUAL (TOKEN)
        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(typeof(GenericResponse<UserDto>), StatusCodes.Status200OK)]
        public async Task<GenericResponse<UserDto>> Me()
        {
            var userId = GetUserId();
            var srv = await userService.Me(userId);
            return ResponseStatus.Ok(HttpContext, srv);
        }

        // ✅ ACTUALIZAR
        [HttpPut("{id:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(GenericResponse<UserDto>), StatusCodes.Status200OK)]
        public async Task<GenericResponse<UserDto>> Update([FromBody] UpdateUserRequest model, Guid id)
        {
            var userId = GetUserId();
            if (id != userId && !CurrentUserHelper.HasPermission(User, PermissionConstants.USERS_MANAGE))
            {
                throw new UnauthorizedAccessException(ResponseConstants.AUTH_CLAIM_USER_NOT_FOUND);
            }

            var srv = await userService.Update(id, model, userId);
            return ResponseStatus.Updated(HttpContext, srv);
        }

        // ✅ ELIMINAR
        [HttpDelete("{id:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(GenericResponse<bool>), StatusCodes.Status200OK)]
        public async Task<GenericResponse<bool>> Delete(Guid id)
        {
            var userId = GetUserId();
            if (id != userId && !CurrentUserHelper.HasPermission(User, PermissionConstants.USERS_MANAGE))
            {
                throw new UnauthorizedAccessException(ResponseConstants.AUTH_CLAIM_USER_NOT_FOUND);
            }

            var srv = await userService.Delete(id);
            return ResponseStatus.Ok(HttpContext, srv);
        }

        // 🔐 MEJOR IMPLEMENTACIÓN DEL CLAIM
        private Guid GetUserId()
        {
            return CurrentUserHelper.GetRequiredUserId(User);
        }
    }
}
