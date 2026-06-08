using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Steam.Web.Api.Helper;
using SteamApplication.Interfaces.Servicie;
using SteamApplication.Models.Dtos;
using SteamApplication.Models.Request.Catalog;
using SteamApplication.Models.Response;
using SteamShared.Constants;

namespace Steam.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController(IGameService service) : ControllerBase
    {
        [HttpPost]
        [Authorize(Policy = PermissionConstants.GAMES_MANAGE)]
        [EndpointSummary("Crear juego")]
        public async Task<GenericResponse<GameDto>> Create([FromBody] CreateGameRequest model)
        {
            var srv = await service.Create(model);
            return ResponseStatus.Created(HttpContext, srv);
        }

        [HttpGet]
        [AllowAnonymous]
        [EndpointSummary("Listar juegos")]
        public GenericResponse<List<GameCardDto>> Get([FromQuery] FilterGameRequest model)
        {
            var srv = service.Get(model);
            return ResponseStatus.Ok(HttpContext, srv);
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        [EndpointSummary("Obtener juego por id")]
        public async Task<GenericResponse<GameDto>> Get(Guid id)
        {
            var srv = await service.Get(id);
            return ResponseStatus.Ok(HttpContext, srv);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Policy = PermissionConstants.GAMES_MANAGE)]
        [EndpointSummary("Actualizar juego")]
        public async Task<GenericResponse<GameDto>> Update(Guid id, [FromBody] UpdateGameRequest model)
        {
            var srv = await service.Update(id, model);
            return ResponseStatus.Updated(HttpContext, srv);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = PermissionConstants.GAMES_MANAGE)]
        [EndpointSummary("Eliminar juego")]
        public async Task<GenericResponse<bool>> Delete(Guid id)
        {
            var srv = await service.Delete(id);
            return ResponseStatus.Ok(HttpContext, srv);
        }

        [HttpPost("{id:guid}/achievements")]
        [Authorize(Policy = PermissionConstants.GAMES_MANAGE)]
        [EndpointSummary("Crear logro para un juego")]
        public async Task<GenericResponse<AchievementDto>> AddAchievement(Guid id, [FromBody] CreateAchievementRequest model)
        {
            var srv = await service.AddAchievement(id, model);
            return ResponseStatus.Created(HttpContext, srv);
        }

        [HttpPost("{id:guid}/dlcs")]
        [Authorize(Policy = PermissionConstants.GAMES_MANAGE)]
        [EndpointSummary("Crear DLC para un juego")]
        public async Task<GenericResponse<DlcDto>> AddDlc(Guid id, [FromBody] CreateDlcRequest model)
        {
            var srv = await service.AddDlc(id, model);
            return ResponseStatus.Created(HttpContext, srv);
        }

        [HttpPost("{id:guid}/offers")]
        [Authorize(Policy = PermissionConstants.GAMES_MANAGE)]
        [EndpointSummary("Crear oferta para un juego")]
        public async Task<GenericResponse<OfferDto>> AddOffer(Guid id, [FromBody] CreateOfferRequest model)
        {
            var srv = await service.AddOffer(id, model);
            return ResponseStatus.Created(HttpContext, srv);
        }
    }
}
