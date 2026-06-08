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
    [Authorize(Policy = PermissionConstants.WISHLIST_MANAGE)]
    public class WishlistController(IWishlistService service) : ControllerBase
    {
        [HttpGet]
        [EndpointSummary("Listar wishlist del usuario autenticado")]
        public GenericResponse<List<WishlistItemDto>> Get()
        {
            var srv = service.Get(CurrentUserHelper.GetRequiredUserId(User));
            return ResponseStatus.Ok(HttpContext, srv);
        }

        [HttpPost]
        [EndpointSummary("Agregar juego a wishlist")]
        public async Task<GenericResponse<WishlistItemDto>> Add([FromBody] CreateWishlistRequest model)
        {
            var srv = await service.Add(CurrentUserHelper.GetRequiredUserId(User), model);
            return ResponseStatus.Created(HttpContext, srv);
        }

        [HttpDelete("{gameId:guid}")]
        [EndpointSummary("Quitar juego de wishlist")]
        public async Task<GenericResponse<bool>> Remove(Guid gameId)
        {
            var srv = await service.Remove(CurrentUserHelper.GetRequiredUserId(User), gameId);
            return ResponseStatus.Ok(HttpContext, srv);
        }
    }
}
