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
    public class ReviewsController(IReviewService service) : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        [EndpointSummary("Listar reseñas")]
        public GenericResponse<List<ReviewDto>> Get([FromQuery] FilterReviewRequest model)
        {
            var srv = service.Get(model);
            return ResponseStatus.Ok(HttpContext, srv);
        }

        [HttpPost]
        [Authorize(Policy = PermissionConstants.REVIEWS_MANAGE)]
        [EndpointSummary("Crear reseña")]
        public async Task<GenericResponse<ReviewDto>> Create([FromBody] CreateReviewRequest model)
        {
            var srv = await service.Create(CurrentUserHelper.GetRequiredUserId(User), model);
            return ResponseStatus.Created(HttpContext, srv);
        }

        [HttpPut("{reviewId:int}")]
        [Authorize(Policy = PermissionConstants.REVIEWS_MANAGE)]
        [EndpointSummary("Actualizar reseña")]
        public async Task<GenericResponse<ReviewDto>> Update(int reviewId, [FromBody] UpdateReviewRequest model)
        {
            var srv = await service.Update(CurrentUserHelper.GetRequiredUserId(User), reviewId, model);
            return ResponseStatus.Updated(HttpContext, srv);
        }

        [HttpDelete("{reviewId:int}")]
        [Authorize(Policy = PermissionConstants.REVIEWS_MANAGE)]
        [EndpointSummary("Eliminar reseña")]
        public async Task<GenericResponse<bool>> Delete(int reviewId)
        {
            var srv = await service.Delete(CurrentUserHelper.GetRequiredUserId(User), reviewId);
            return ResponseStatus.Ok(HttpContext, srv);
        }

        [HttpPost("{reviewId:int}/answers")]
        [Authorize(Policy = PermissionConstants.REVIEWS_MANAGE)]
        [EndpointSummary("Responder una reseña")]
        public async Task<GenericResponse<ReviewAnswerDto>> Answer(int reviewId, [FromBody] CreateReviewAnswerRequest model)
        {
            var srv = await service.Answer(CurrentUserHelper.GetRequiredUserId(User), reviewId, model);
            return ResponseStatus.Created(HttpContext, srv);
        }
    }
}
