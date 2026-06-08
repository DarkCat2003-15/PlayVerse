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
    public class PublishersController(IPublisherService service) : ControllerBase
    {
        [HttpPost]
        [Authorize(Policy = PermissionConstants.CATALOG_MANAGE)]
        [EndpointSummary("Crear publisher")]
        public async Task<GenericResponse<PublisherDto>> Create([FromBody] CreatePublisherRequest model)
        {
            var srv = await service.Create(model);
            return ResponseStatus.Created(HttpContext, srv);
        }

        [HttpGet]
        [AllowAnonymous]
        [EndpointSummary("Listar publishers")]
        public GenericResponse<List<PublisherDto>> Get([FromQuery] FilterPublisherRequest model)
        {
            var srv = service.Get(model);
            return ResponseStatus.Ok(HttpContext, srv);
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        [EndpointSummary("Obtener publisher por id")]
        public async Task<GenericResponse<PublisherDto>> Get(Guid id)
        {
            var srv = await service.Get(id);
            return ResponseStatus.Ok(HttpContext, srv);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Policy = PermissionConstants.CATALOG_MANAGE)]
        [EndpointSummary("Actualizar publisher")]
        public async Task<GenericResponse<PublisherDto>> Update(Guid id, [FromBody] UpdatePublisherRequest model)
        {
            var srv = await service.Update(id, model);
            return ResponseStatus.Updated(HttpContext, srv);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = PermissionConstants.CATALOG_MANAGE)]
        [EndpointSummary("Eliminar publisher")]
        public async Task<GenericResponse<bool>> Delete(Guid id)
        {
            var srv = await service.Delete(id);
            return ResponseStatus.Ok(HttpContext, srv);
        }
    }
}
