using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Steam.Web.Api.Helper;
using SteamApplication.Interfaces.Servicie;
using SteamApplication.Models.Dtos;
using SteamApplication.Models.Request.Auth.RecoverPassword;
using SteamApplication.Models.Request.Auth.Register;
using SteamApplication.Models.Request.Users;
using SteamApplication.Models.Response;
using SteamApplication.Models.Responses.Auth;
using SteamShared.Constants;

namespace Steam.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService service) : ControllerBase
    {
        [HttpPost("login")]
        [EndpointSummary("Iniciar sesión como usuario")]
        [EndpointDescription("Autentica al usuario y devuelve JWT, refresh token y el estado del usuario. StatusId: 1 = Activo, 0 = Desactivo.")]
        [ProducesResponseType<GenericResponse<string>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<GenericResponse<LoginAuthResponse>>(StatusCodes.Status200OK)]
        [Tags("auth", "users", "jwt", "refresh_token")]
        public async Task<GenericResponse<LoginAuthResponse>> Login([FromBody] LoginAuthRequest model)
        {
            var srv = await service.Login(model);
            return ResponseStatus.Ok(HttpContext, srv);
        }

        [HttpPost("renew")]
        [EndpointSummary("Renovar sesión como usuario")]
        [EndpointDescription("Renueva la sesión del usuario y devuelve nuevos tokens junto con el estado actual del usuario. StatusId: 1 = Activo, 0 = Desactivo.")]
        [ProducesResponseType<GenericResponse<string>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<GenericResponse<LoginAuthResponse>>(StatusCodes.Status200OK)]
        [Tags("auth", "users", "jwt", "refresh_token")]
        public async Task<GenericResponse<LoginAuthResponse>> Renew([FromBody] RenewAuthRequest model)
        {
            var srv = await service.Renew(model);
            return ResponseStatus.Ok(HttpContext, srv);
        }

        [HttpPost("register/init")]
        [EndpointSummary("Comenzar proceso de registro")]
        [ProducesResponseType<GenericResponse<string>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<GenericResponse<string>>(StatusCodes.Status200OK)]
        [Tags("auth", "users", "register")]
        public async Task<GenericResponse<string>> RegisterInit([FromBody] RegisterInitAuthRequest model)
        {
            var srv = await service.RegisterInit(model);
            return ResponseStatus.Ok(HttpContext, srv);
        }

        [HttpGet("register/validate/{token}")]
        [EndpointSummary("Validar token de registro")]
        [ProducesResponseType<GenericResponse<string>>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<GenericResponse<RegisterInitAuthResponse>>(StatusCodes.Status200OK)]
        [Tags("auth", "users", "register", "validate")]
        public async Task<GenericResponse<RegisterInitAuthResponse>> RegisterValidateToken(string token)
        {
            var srv = await service.RegisterValidateToken(token);
            return ResponseStatus.Ok(HttpContext, srv);
        }

        [HttpPost("register/complete/{token}")]
        [EndpointSummary("Completar el proceso de registro")]
        [EndpointDescription("Completa el registro del usuario y lo crea con StatusId = 1 (Activo).")]
        [ProducesResponseType<GenericResponse<string>>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<GenericResponse<UserDto>>(StatusCodes.Status201Created)]
        [Tags("auth", "users", "register", "complete")]
        public async Task<GenericResponse<UserDto>> RegisterComplete([FromBody] CreateUsersRequest model, string token)
        {
            var srv = await service.RegisterComplete(model, token);
            return ResponseStatus.Created(HttpContext, srv);
        }

        [HttpPost("recoverPassword")]
        [EndpointSummary("Enviar código OTP para recuperación de contraseña")]
        [ProducesResponseType<GenericResponse<string>>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<GenericResponse<string>>(StatusCodes.Status200OK)]
        [Tags("auth", "users", "recover_password")]
        public async Task<GenericResponse<string>> RecoverPasswordSendOTP([FromBody] RecoverPasswordSendOTPAuthRequest model)
        {
            var srv = await service.RecoverPasswordSendOTP(model);
            return ResponseStatus.Ok(HttpContext, srv);
        }

        [HttpPost("recoverPassword/{code}")]
        [EndpointSummary("Cambiar contraseña con código OTP")]
        [ProducesResponseType<GenericResponse<string>>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<GenericResponse<string>>(StatusCodes.Status200OK)]
        [Tags("auth", "users", "recover_password", "change_password")]
        public async Task<GenericResponse<string>> RecoverPassword([FromBody] RecoverPasswordAuthRequest model, string code)
        {
            var srv = await service.RecoverPassword(model, code);
            return ResponseStatus.Ok(HttpContext, srv);
        }

        [HttpPost("changePassword")]
        [Authorize]
        [EndpointSummary("Cambiar la contraseña del usuario autenticado")]
        [ProducesResponseType<GenericResponse<string>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<GenericResponse<string>>(StatusCodes.Status200OK)]
        [Tags("auth", "users", "change_password")]
        public async Task<GenericResponse<string>> ChangePassword([FromBody] ChangePasswordAuthRequest model)
        {
            var srv = await service.ChangePassword(model, GetUserId());
            return ResponseStatus.Ok(HttpContext, srv);
        }

        private Guid GetUserId()
        {
            var claim = User.FindFirst(ClaimsConstants.USERS_ID);

            if (claim == null || !Guid.TryParse(claim.Value, out var userId))
                throw new UnauthorizedAccessException(ResponseConstants.AUTH_CLAIM_USER_NOT_FOUND);

            return userId;
        }
    }
}
