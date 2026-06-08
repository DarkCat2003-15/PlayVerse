using SteamApplication.Interfaces.Servicie;
using SteamApplication.Models.Dtos;
using SteamApplication.Servicios.EmailTemplates;
using SteamDomain.Database.SqlServer;

namespace SteamApplication.Servicios
{
    public class EmailTemplateService(EmailTemplateData data, IUnitOfWork uow) : IEmailTemplateService
    {
        public Task<EmailTemplateDTO> Get(string name, Dictionary<string, string> variables)
        {
            var template = data.Data.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (template == null)
                throw new Exception($"Template no encontrado: {name}");

            var body = template.Body;

            foreach (var variable in variables)
            {
                body = body.Replace("{{" + variable.Key + "}}", variable.Value);
            }

            return Task.FromResult(new EmailTemplateDTO
            {
                Body = body,
                Subject = template.Subject
            });
        }

        public async Task Init()
        {
            var templates = await uow.emailTemplateRepository.Get();

            if (templates.Count > 0)
            {
                data.Data = templates;
            }
        }
    }
}
