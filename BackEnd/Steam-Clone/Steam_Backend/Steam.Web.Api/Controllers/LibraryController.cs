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
    [Authorize(Policy = PermissionConstants.LIBRARY_PURCHASE)]
    public class LibraryController(ILibraryService service) : ControllerBase
    {
        [HttpGet]
        [EndpointSummary("Listar biblioteca del usuario autenticado")]
        public GenericResponse<List<LibraryItemDto>> Get()
        {
            var srv = service.Get(CurrentUserHelper.GetRequiredUserId(User));
            return ResponseStatus.Ok(HttpContext, srv);
        }

        [HttpPost("purchase")]
        [EndpointSummary("Comprar juego para la biblioteca")]
        public async Task<GenericResponse<LibraryItemDto>> Purchase([FromBody] PurchaseGameRequest model)
        {
            var srv = await service.Purchase(CurrentUserHelper.GetRequiredUserId(User), model);
            return ResponseStatus.Created(HttpContext, srv);
        }
    }
}
