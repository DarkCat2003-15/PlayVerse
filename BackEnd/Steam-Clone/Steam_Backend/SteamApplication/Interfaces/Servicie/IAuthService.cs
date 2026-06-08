using SteamApplication.Models.Dtos;
using SteamApplication.Models.Request.Auth.RecoverPassword;
using SteamApplication.Models.Request.Auth.Register;
using SteamApplication.Models.Request.Users;
using SteamApplication.Models.Response;
using SteamApplication.Models.Responses.Auth;

namespace SteamApplication.Interfaces.Servicie
{
    public interface IAuthService
    {
        Task<GenericResponse<LoginAuthResponse>> Login(LoginAuthRequest model);
        Task<GenericResponse<LoginAuthResponse>> Renew(RenewAuthRequest model);

        Task<GenericResponse<string>> RegisterInit(RegisterInitAuthRequest model);
        Task<GenericResponse<RegisterInitAuthResponse>> RegisterValidateToken(string token);
        Task<GenericResponse<UserDto>> RegisterComplete(CreateUsersRequest model, string token);

        Task<GenericResponse<string>> RecoverPasswordSendOTP(RecoverPasswordSendOTPAuthRequest model);
        Task<GenericResponse<string>> RecoverPassword(RecoverPasswordAuthRequest model, string code);
        Task<GenericResponse<string>> ChangePassword(ChangePasswordAuthRequest model, Guid userId);
    }
}
