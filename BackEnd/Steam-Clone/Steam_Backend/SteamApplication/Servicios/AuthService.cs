using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SteamApplication.Helpers;
using SteamApplication.Interfaces.Servicie;
using SteamApplication.Models.Dtos;
using SteamApplication.Models.Helpers;
using SteamApplication.Models.Request.Auth.RecoverPassword;
using SteamApplication.Models.Request.Auth.Register;
using SteamApplication.Models.Request.Users;
using SteamApplication.Models.Response;
using SteamApplication.Models.Responses.Auth;
using SteamDomain.Database.SqlServer;
using SteamDomain.Database.SqlServer.Entities;
using SteamDomain.Exceptions;
using SteamShared.Constants;
using SteamShared.Helpers;

namespace SteamApplication.Servicios
{
    public class AuthService(
        IUnitOfWork uow,
        IConfiguration configuration,
        ICacheService cacheService,
        IEmailTemplateService emailTemplateService,
        SMTP smtp,
        ILogger<AuthService> logger) : IAuthService
    {
        public async Task<GenericResponse<LoginAuthResponse>> Login(LoginAuthRequest model)
        {
            var user = await GetUserByEmail(model.Email, ResponseConstants.AUTH_USER_OR_PASSWORD_NOT_FOUND);
            ValidatePassword(model.Password, user.Password);
            var authData = await GetAuthData(user.UserId);

            var token = TokenHelper.Create(user.UserId, authData.Roles, authData.Permissions, configuration, cacheService);
            var refreshToken = TokenHelper.CreateRefresh(user.UserId, configuration, cacheService);

            await TrySendEmail(
                model.Email,
                EmailTemplateNameConstants.AUTH_LOGIN_SUCCESS,
                new Dictionary<string, string>
                {
                    { "datetime", DateTimeHelper.UtcNow().ToString("u") }
                });

            return ResponseHelper.Create(CreateLoginResponse(user, token, refreshToken, authData.Roles, authData.Permissions));
        }

        public async Task<GenericResponse<LoginAuthResponse>> Renew(RenewAuthRequest model)
        {
            var findRefreshToken = cacheService.Get<RefreshToken>(CacheHelpers.AuthRefreshTokenKey(model.RefreshToken))
                ?? throw new NotFoundException(ResponseConstants.AUTH_REFRESH_TOKEN_NOT_FOUND);

            cacheService.Delete(CacheHelpers.AuthRefreshTokenKey(model.RefreshToken));

            var user = await GetUserById(findRefreshToken.UserId);
            var authData = await GetAuthData(user.UserId);
            var token = TokenHelper.Create(findRefreshToken.UserId, authData.Roles, authData.Permissions, configuration, cacheService);
            var refreshToken = TokenHelper.CreateRefresh(findRefreshToken.UserId, configuration, cacheService);

            return ResponseHelper.Create(CreateLoginResponse(user, token, refreshToken, authData.Roles, authData.Permissions));
        }

        public async Task<GenericResponse<string>> RegisterInit(RegisterInitAuthRequest model)
        {
            if (await uow.userRepository.IfExists(model.Email))
                throw new BadRequestException(ResponseConstants.USER_EMAIL_TAKED);

            var token = Generate.RandomText(80);
            var cacheKey = CacheHelpers.AuthRegisterTokenCreation(token);

            cacheService.Create(
                cacheKey.Key,
                cacheKey.Expiration,
                new RegisterTokenData { Email = model.Email });

            await TrySendEmail(
                model.Email,
                EmailTemplateNameConstants.AUTH_REGISTER_INIT,
                new Dictionary<string, string>
                {
                    { "token", token }
                });

            return ResponseHelper.Create(
                data: token,
                message: "Se genero el token de registro correctamente.");
        }

        public Task<GenericResponse<RegisterInitAuthResponse>> RegisterValidateToken(string token)
        {
            var registerToken = cacheService.Get<RegisterTokenData>(CacheHelpers.AuthRegisterTokenKey(token))
                ?? throw new NotFoundException(ResponseConstants.AUTH_REGISTER_TOKEN_NOT_FOUND);

            return Task.FromResult(ResponseHelper.Create(
                new RegisterInitAuthResponse
                {
                    Email = registerToken.Email
                },
                message: "El token de registro es valido."));
        }

        public async Task<GenericResponse<UserDto>> RegisterComplete(CreateUsersRequest model, string token)
        {
            var registerToken = cacheService.Get<RegisterTokenData>(CacheHelpers.AuthRegisterTokenKey(token))
                ?? throw new NotFoundException(ResponseConstants.AUTH_REGISTER_TOKEN_NOT_FOUND);

            if (!string.Equals(model.Email, registerToken.Email, StringComparison.OrdinalIgnoreCase))
                throw new BadRequestException(ResponseConstants.AUTH_REGISTER_EMAIL_MISMATCH);

            if (await uow.userRepository.IfExists(model.Email))
                throw new BadRequestException(ResponseConstants.USER_EMAIL_TAKED);

            var user = await uow.userRepository.Create(new User
            {
                Email = model.Email,
                UserName = model.Username,
                Password = Hasher.HashPassword(model.Password),
                StatusId = UserStatusConstants.ActiveId,
                UpdateAt = DateTimeHelper.UtcNow()
            });

            await TryAssignDefaultRole(user.UserId, RoleConstants.User);

            cacheService.Delete(CacheHelpers.AuthRegisterTokenKey(token));

            await TrySendEmail(
                model.Email,
                EmailTemplateNameConstants.USER_REGISTER,
                new Dictionary<string, string>
                {
                    { "password", model.Password }
                });

            return ResponseHelper.Create(await Map(user), message: "Usuario registrado correctamente.");
        }

        public async Task<GenericResponse<string>> RecoverPasswordSendOTP(RecoverPasswordSendOTPAuthRequest model)
        {
            var user = await GetUserByEmail(model.Email, ResponseConstants.USER_NOT_EXISTS);
            var code = Generate.NumericCode();
            var cacheKey = CacheHelpers.AuthRecoverPasswordCodeCreation(code);

            cacheService.Create(
                cacheKey.Key,
                cacheKey.Expiration,
                new RecoverPasswordCodeData
                {
                    UserId = user.UserId,
                    Email = user.Email!
                });

            await TrySendEmail(
                user.Email!,
                EmailTemplateNameConstants.AUTH_RECOVER_PASSWORD_OTP,
                new Dictionary<string, string>
                {
                    { "code", code }
                });

            return ResponseHelper.Create(
                data: code,
                message: "Se envio el codigo OTP para recuperar la contrasena.");
        }

        public async Task<GenericResponse<string>> RecoverPassword(RecoverPasswordAuthRequest model, string code)
        {
            var recoverData = cacheService.Get<RecoverPasswordCodeData>(CacheHelpers.AuthRecoverPasswordCodeKey(code))
                ?? throw new NotFoundException(ResponseConstants.AUTH_RECOVER_PASSWORD_CODE_NOT_FOUND);

            var user = await GetUserById(recoverData.UserId);

            EnsureNewPassword(user.Password, model.NewPassword);

            user.Password = Hasher.HashPassword(model.NewPassword);
            user.StatusId = UserStatusConstants.ActiveId;
            user.UpdateAt = DateTimeHelper.UtcNow();
            await uow.userRepository.Update(user);

            cacheService.Delete(CacheHelpers.AuthRecoverPasswordCodeKey(code));

            await TrySendEmail(
                recoverData.Email,
                EmailTemplateNameConstants.AUTH_PASSWORD_CHANGED,
                new Dictionary<string, string>());

            return ResponseHelper.Create(
                data: "Contraseña actualizada correctamente.",
                message: "Contraseña actualizada correctamente.");
        }

        public async Task<GenericResponse<string>> ChangePassword(ChangePasswordAuthRequest model, Guid userId)
        {
            var user = await GetUserById(userId);
            ValidatePassword(model.CurrentPassword, user.Password, ResponseConstants.AUTH_CURRENT_PASSWORD_INVALID);
            EnsureNewPassword(user.Password, model.NewPassword);

            user.Password = Hasher.HashPassword(model.NewPassword);
            user.StatusId = UserStatusConstants.ActiveId;
            user.UpdateAt = DateTimeHelper.UtcNow();
            await uow.userRepository.Update(user);

            await TrySendEmail(
                user.Email!,
                EmailTemplateNameConstants.AUTH_PASSWORD_CHANGED,
                new Dictionary<string, string>());

            return ResponseHelper.Create(
                data: "Contraseña cambiada correctamente.",
                message: "Contraseña cambiada correctamente.");
        }

        private async Task<User> GetUserByEmail(string email, string notFoundMessage)
        {
            return await uow.userRepository.Get(email)
                ?? throw new BadRequestException(notFoundMessage);
        }

        private async Task<User> GetUserById(Guid userId)
        {
            return await uow.userRepository.Get(userId)
                ?? throw new NotFoundException(ResponseConstants.USER_NOT_EXISTS);
        }

        private static void ValidatePassword(string plainPassword, string? hashedPassword, string? message = null)
        {
            var isValid = !string.IsNullOrWhiteSpace(hashedPassword)
                && Hasher.ComparePassword(plainPassword, hashedPassword);

            if (!isValid)
                throw new BadRequestException(message ?? ResponseConstants.AUTH_USER_OR_PASSWORD_NOT_FOUND);
        }

        private static void EnsureNewPassword(string? currentHashedPassword, string newPassword)
        {
            if (!string.IsNullOrWhiteSpace(currentHashedPassword) && Hasher.ComparePassword(newPassword, currentHashedPassword))
                throw new BadRequestException(ResponseConstants.AUTH_PASSWORD_ALREADY_USED);
        }

        private async Task TrySendEmail(string email, string templateName, Dictionary<string, string> variables)
        {
            try
            {
                var template = await emailTemplateService.Get(templateName, variables);
                await smtp.Send(email, template.Subject, template.Body);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "No se pudo enviar el correo usando la plantilla {TemplateName} al destinatario {Email}", templateName, email);
            }
        }

        private async Task<UserDto> Map(User user)
        {
            var statusId = user.StatusId ?? (user.DeletedAt == null ? UserStatusConstants.ActiveId : UserStatusConstants.InactiveId);
            var roles = await uow.roleRepository.GetByUserId(user.UserId);

            return new UserDto
            {
                UserId = user.UserId,
                Username = user.UserName,
                Email = user.Email ?? string.Empty,
                CreatedAt = user.CreatedAt ?? DateTimeHelper.UtcNow(),
                StatusId = statusId,
                StatusName = UserStatusConstants.ResolveName(statusId),
                IsActive = UserStatusConstants.IsActive(statusId),
                Roles = roles.Select(x => x.Name).ToList()
            };
        }

        private static LoginAuthResponse CreateLoginResponse(User user, string token, string refreshToken, List<string> roles, List<string> permissions)
        {
            var statusId = user.StatusId ?? (user.DeletedAt == null ? UserStatusConstants.ActiveId : UserStatusConstants.InactiveId);

            return new LoginAuthResponse
            {
                Token = token,
                RefreshToken = refreshToken,
                UserId = user.UserId,
                Email = user.Email ?? string.Empty,
                Username = user.UserName,
                StatusId = statusId,
                StatusName = UserStatusConstants.ResolveName(statusId),
                IsActive = UserStatusConstants.IsActive(statusId),
                Roles = roles,
                Permissions = permissions
            };
        }

        private async Task<(List<string> Roles, List<string> Permissions)> GetAuthData(Guid userId)
        {
            var roles = await uow.roleRepository.GetByUserId(userId);
            var permissions = await uow.roleRepository.GetPermissionsByUserId(userId);

            return (
                roles.Select(x => x.Name).Distinct().ToList(),
                permissions.Select(x => x.Code).Distinct().ToList()
            );
        }

        private async Task TryAssignDefaultRole(Guid userId, string roleName)
        {
            try
            {
                var role = await uow.roleRepository.GetByName(roleName);
                if (role == null || await uow.roleRepository.AssignmentExists(userId, role.Id))
                {
                    return;
                }

                await uow.roleRepository.AssignRole(userId, role.Id, null);
                await uow.SaveChangesAsync();
            }
            catch
            {
            }
        }
    }
}
