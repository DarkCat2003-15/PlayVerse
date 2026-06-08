using Microsoft.Extensions.Configuration;
using SteamApplication.Models.Helpers;
using SteamShared.Constants;

namespace SteamApplication.Helpers
{
    public class CacheHelpers
    {
        public static readonly Random rnd = new();

        public static string AuthTokenKey(string value)
        {
            return $"auth:tokens:{value}";
        }

        public static CacheKey AuthTokenCreation(string value, TimeSpan expiration)
        {
            return new CacheKey
            {
                Key = AuthTokenKey(value),
                Expiration = expiration
            };
        }

        public static string AuthRefreshTokenKey(string value)
        {
            return $"auth:refresh_tokens:{value}";
        }

        public static CacheKey AuthRefreshTokenCreation(string value, IConfiguration configuration)
        {
            return new CacheKey
            {
                Key = AuthRefreshTokenKey(value),
                Expiration = TimeSpan.FromDays(Convert.ToInt32(configuration[ConfigurationConstants.AUTH_REFRESH_TOKEN_EXPIRATION_IN_DAYS] ?? "15"))
            };
        }

        public static string AuthRegisterTokenKey(string value)
        {
            return $"auth:register_tokens:{value}";
        }

        public static CacheKey AuthRegisterTokenCreation(string value, int expirationInHours = 24)
        {
            return new CacheKey
            {
                Key = AuthRegisterTokenKey(value),
                Expiration = TimeSpan.FromHours(expirationInHours)
            };
        }

        public static string AuthRecoverPasswordCodeKey(string value)
        {
            return $"auth:recover_password_codes:{value}";
        }

        public static CacheKey AuthRecoverPasswordCodeCreation(string value, int expirationInMinutes = 15)
        {
            return new CacheKey
            {
                Key = AuthRecoverPasswordCodeKey(value),
                Expiration = TimeSpan.FromMinutes(expirationInMinutes)
            };
        }
    }
}
