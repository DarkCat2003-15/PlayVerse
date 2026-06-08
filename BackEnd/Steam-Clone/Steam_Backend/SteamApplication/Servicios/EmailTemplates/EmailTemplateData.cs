using SteamDomain.Database.SqlServer.Entities;
using SteamShared.Constants;

namespace SteamApplication.Servicios.EmailTemplates
{
    public class EmailTemplateData
    {
        public List<EmailTemplate> Data { get; set; } =
        [
            new EmailTemplate
            {
                Name = EmailTemplateNameConstants.USER_REGISTER,
                Subject = "Bienvenido a PlayVerse",
                Body = "Tu cuenta fue creada correctamente."
            },
            new EmailTemplate
            {
                Name = EmailTemplateNameConstants.AUTH_LOGIN_SUCCESS,
                Subject = "Inicio de sesión correcto",
                Body = "Tu inicio de sesión fue exitoso el {{datetime}}."
            },
            new EmailTemplate
            {
                Name = EmailTemplateNameConstants.AUTH_LOGIN_FAILED,
                Subject = "Intento de inicio de sesión fallido",
                Body = "Se detectó un intento fallido de inicio de sesión en tu cuenta."
            },
            new EmailTemplate
            {
                Name = EmailTemplateNameConstants.AUTH_REGISTER_INIT,
                Subject = "Token de registro",
                Body = "Usa este token para completar tu registro: {{token}}"
            },
            new EmailTemplate
            {
                Name = EmailTemplateNameConstants.AUTH_RECOVER_PASSWORD_OTP,
                Subject = "Código de recuperación",
                Body = "Tu código OTP para recuperar la contraseña es: {{code}}"
            },
            new EmailTemplate
            {
                Name = EmailTemplateNameConstants.AUTH_PASSWORD_CHANGED,
                Subject = "Contraseña actualizada",
                Body = "Tu contraseña fue actualizada correctamente."
            }
        ];
    }
}
