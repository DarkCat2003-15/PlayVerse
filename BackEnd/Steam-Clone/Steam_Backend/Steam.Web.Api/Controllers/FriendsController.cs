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
    [Authorize(Policy = PermissionConstants.FRIENDS_MANAGE)]
    public class FriendsController(IFriendService service) : ControllerBase
    {
        [HttpGet]
        [EndpointSummary("Listar amigos del usuario autenticado")]
        public GenericResponse<List<FriendDto>> Get()
        {
            var srv = service.Get(CurrentUserHelper.GetRequiredUserId(User));
            return ResponseStatus.Ok(HttpContext, srv);
        }

        [HttpPost]
        [EndpointSummary("Agregar amigo")]
        public async Task<GenericResponse<FriendDto>> Add([FromBody] CreateFriendRequest model)
        {
            var srv = await service.Add(CurrentUserHelper.GetRequiredUserId(User), model);
            return ResponseStatus.Created(HttpContext, srv);
        }

        [HttpDelete("{friendId:guid}")]
        [EndpointSummary("Eliminar amigo")]
        public async Task<GenericResponse<bool>> Remove(Guid friendId)
        {
            var srv = await service.Remove(CurrentUserHelper.GetRequiredUserId(User), friendId);
            return ResponseStatus.Ok(HttpContext, srv);
        }
    }
}
