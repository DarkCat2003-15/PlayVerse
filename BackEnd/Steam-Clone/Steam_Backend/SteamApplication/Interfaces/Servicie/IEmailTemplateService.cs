using SteamApplication.Models.Dtos;

namespace SteamApplication.Interfaces.Servicie
{
    public interface IEmailTemplateService
    {
        Task<EmailTemplateDTO> Get(string name, Dictionary<string, string> variables);
        Task Init();
    }
}
