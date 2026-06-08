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
    public class GenresController(IGenreService service) : ControllerBase
    {
        [HttpPost]
        [Authorize(Policy = PermissionConstants.CATALOG_MANAGE)]
        [EndpointSummary("Crear genero")]
        public async Task<GenericResponse<GenreDto>> Create([FromBody] CreateGenreRequest model)
        {
            var srv = await service.Create(model);
            return ResponseStatus.Created(HttpContext, srv);
        }

        [HttpGet]
        [AllowAnonymous]
        [EndpointSummary("Listar generos")]
        public GenericResponse<List<GenreDto>> Get([FromQuery] FilterGenreRequest model)
        {
            var srv = service.Get(model);
            return ResponseStatus.Ok(HttpContext, srv);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        [EndpointSummary("Obtener genero por id")]
        public async Task<GenericResponse<GenreDto>> Get(int id)
        {
            var srv = await service.Get(id);
            return ResponseStatus.Ok(HttpContext, srv);
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = PermissionConstants.CATALOG_MANAGE)]
        [EndpointSummary("Actualizar genero")]
        public async Task<GenericResponse<GenreDto>> Update(int id, [FromBody] UpdateGenreRequest model)
        {
            var srv = await service.Update(id, model);
            return ResponseStatus.Updated(HttpContext, srv);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = PermissionConstants.CATALOG_MANAGE)]
        [EndpointSummary("Eliminar genero")]
        public async Task<GenericResponse<bool>> Delete(int id)
        {
            var srv = await service.Delete(id);
            return ResponseStatus.Ok(HttpContext, srv);
        }
    }
}
