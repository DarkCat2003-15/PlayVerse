using Microsoft.AspNetCore.Mvc;
using Steam.Web.Api.Attributes;
using Steam.Web.Api.Helper;
using SteamApplication.Interfaces.Servicie;
using SteamApplication.Models.Dtos;
using SteamApplication.Models.Response;

namespace Steam.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [DeveloperAuthor(Name = "Juan B.", Description = "Esto lo cree para la información de la APP")]
    public class AppController(IAppService appService) : ControllerBase
    {
        [HttpGet("info")]
        [EndpointSummary("Información de la aplicación")]
        [EndpointDescription("Los roles, permisos, versión y mas detalles de la aplicación")]
        [ProducesResponseType<GenericResponse<AppInfoDto>>(StatusCodes.Status200OK)]
        public async Task<GenericResponse<AppInfoDto>> Info()
        {
            var srv = await appService.Info();
            return ResponseStatus.Ok(HttpContext, srv);
        }





    }
}
