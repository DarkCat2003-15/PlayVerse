using Microsoft.EntityFrameworkCore;
using SteamDomain.Database.SqlServer.Context;
using SteamDomain.Database.SqlServer.Entities;
using SteamDomain.Interfaces.Repositories;

namespace SteamInfrastructure.Persistence.SqlServer.Repositories
{
    public class EmailTemplateRepository(SteamContext context) : IEmailTemplateRepository
    {
        public async Task<List<EmailTemplate>> Get()
        {
            return await context.EmailTemplates.ToListAsync();
        }
    }
}
