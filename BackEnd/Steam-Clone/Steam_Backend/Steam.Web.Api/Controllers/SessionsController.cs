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
    [Authorize(Policy = PermissionConstants.SESSIONS_PLAY)]
    public class SessionsController(IGameSessionService service) : ControllerBase
    {
        [HttpGet]
        [EndpointSummary("Listar sesiones del usuario autenticado")]
        public GenericResponse<List<GameSessionDto>> Get([FromQuery] FilterSessionRequest model)
        {
            var srv = service.Get(CurrentUserHelper.GetRequiredUserId(User), model);
            return ResponseStatus.Ok(HttpContext, srv);
        }

        [HttpPost("start")]
        [EndpointSummary("Iniciar sesion de juego")]
        public async Task<GenericResponse<GameSessionDto>> Start([FromBody] CreateSessionRequest model)
        {
            var srv = await service.Start(CurrentUserHelper.GetRequiredUserId(User), model);
            return ResponseStatus.Created(HttpContext, srv);
        }

        [HttpPost("{sessionId:int}/end")]
        [EndpointSummary("Finalizar sesion de juego")]
        public async Task<GenericResponse<GameSessionDto>> End(int sessionId)
        {
            var srv = await service.End(CurrentUserHelper.GetRequiredUserId(User), sessionId);
            return ResponseStatus.Updated(HttpContext, srv);
        }
    }
}
