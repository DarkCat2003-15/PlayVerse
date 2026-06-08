using SteamDomain.Database.SqlServer.Entities;

namespace SteamDomain.Interfaces.Repositories
{
    public interface IEmailTemplateRepository
    {
        Task<List<EmailTemplate>> Get();
    }
}
