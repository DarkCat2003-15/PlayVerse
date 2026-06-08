using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Steam.Web.Api.Helper;
using SteamApplication.Interfaces.Servicie;
using SteamApplication.Models.Dtos;
using SteamApplication.Models.Request.Community;
using SteamApplication.Models.Response;
using SteamShared.Constants;

namespace Steam.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = PermissionConstants.ACHIEVEMENTS_UNLOCK)]
    public class UserAchievementsController(IUserAchievementService service) : ControllerBase
    {
        [HttpGet]
        [EndpointSummary("Listar logros desbloqueados del usuario autenticado")]
        public GenericResponse<List<UserAchievementDto>> Get([FromQuery] FilterUserAchievementRequest model)
        {
            var srv = service.Get(CurrentUserHelper.GetRequiredUserId(User), model);
            return ResponseStatus.Ok(HttpContext, srv);
        }

        [HttpPost("unlock")]
        [EndpointSummary("Desbloquear logro para el usuario autenticado")]
        public async Task<GenericResponse<UserAchievementDto>> Unlock([FromBody] UnlockAchievementRequest model)
        {
            var srv = await service.Unlock(CurrentUserHelper.GetRequiredUserId(User), model);
            return ResponseStatus.Created(HttpContext, srv);
        }
    }
}
