using Microsoft.Extensions.Configuration;
using SteamApplication.Helpers;
using SteamApplication.Interfaces.Servicie;
using SteamApplication.Models.Dtos;
using SteamApplication.Models.Response;
using SteamShared.Constants;

namespace SteamApplication.Servicios
{
    public class AppService(IConfiguration configuration) : IAppService
    {
        public async Task<GenericResponse<AppInfoDto>> Info()
        {
            return ResponseHelper.Create(new AppInfoDto
            {
                Name = "PlayVerse",
                Slogan = "Tu universo de videojuegos en un solo lugar",
                Version = configuration[ConfigurationConstants.VERSION] ?? "0.0.0"
            });
        }
    }
}
