using SteamApplication.Models.Dtos;
using SteamApplication.Models.Response;

namespace SteamApplication.Interfaces.Servicie
{
    public interface IAppService
    {
        Task<GenericResponse<AppInfoDto>> Info();
    }
}
