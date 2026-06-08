using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SteamApplication.Interfaces.Servicie;
using SteamApplication.Models.Helpers;
using SteamShared.Constants;
using SteamShared.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SteamApplication.Helpers
{
    public static class TokenHelper
    {
        public static readonly Random rnd = new();
        public static string Create(Guid userId, IEnumerable<string> roles, IEnumerable<string> permissions, IConfiguration configuration, ICacheService cache)
        {
            var tokenConfiguration = Configuration(configuration);
            var signingCredentials = new SigningCredentials(tokenConfiguration.SecurityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimsConstants.USERS_ID, userId.ToString())
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimsConstants.ROLE, role)));
            claims.AddRange(permissions.Select(permission => new Claim(ClaimsConstants.PERMISSION, permission)));

            var securityToken = new JwtSecurityToken(
                audience: tokenConfiguration.Audience,
                issuer: tokenConfiguration.Issuer,
                expires: tokenConfiguration.Expiration,
                signingCredentials: signingCredentials,
                claims: claims
                );
            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            var cacheKey = CacheHelpers.AuthTokenCreation(token, tokenConfiguration.ExpirationTimeSpan);
            cache.Create(cacheKey.Key, cacheKey.Expiration, token);

            return token;
        }

        public static string CreateRefresh(Guid userId, IConfiguration configuration, ICacheService cacheService)
        {
            var token = Generate.RandomText(100);
            var cacheKey = CacheHelpers.AuthRefreshTokenCreation(token, configuration);

            cacheService.Create(cacheKey.Key, cacheKey.Expiration, new RefreshToken
            {
                UserId = userId,
                ExpirationInDays = cacheKey.Expiration
            });

            return token;
        }



        public static TokenConfiguration Configuration(IConfiguration configuration)
        {
            var issuer = Environment.GetEnvironmentVariable(ConfigurationConstants.JWT_ISSUER)
                ?? configuration[ConfigurationConstants.JWT_ISSUER]
                ?? throw new Exception(ResponseConstants.ConfigurationPropertyNotFound(ConfigurationConstants.JWT_ISSUER));

            var audience = Environment.GetEnvironmentVariable(ConfigurationConstants.JWT_AUDIENCE)
                ?? configuration[ConfigurationConstants.JWT_AUDIENCE]
                ?? throw new Exception(ResponseConstants.ConfigurationPropertyNotFound(ConfigurationConstants.JWT_AUDIENCE));

            var privateKey = Environment.GetEnvironmentVariable(ConfigurationConstants.JWT_PRIVATE_KEY)
                ?? configuration[ConfigurationConstants.JWT_PRIVATE_KEY]
                ?? throw new Exception(ResponseConstants.ConfigurationPropertyNotFound(ConfigurationConstants.JWT_PRIVATE_KEY));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey));

            var now = DateTimeHelper.UtcNow();
            var randomExpiration = rnd.Next(Convert.ToInt32(configuration[ConfigurationConstants.JWT_EXPIRATION_IN_MINUTES_MIN] ?? "1"), Convert.ToInt32(configuration[ConfigurationConstants.JWT_EXPIRATION_IN_MINUTES_MAX] ?? "5"));
            var timespanExpiration = TimeSpan.FromMinutes(randomExpiration);
            var datetimeExpiration = now.Add(TimeSpan.FromMinutes(randomExpiration));

            return new TokenConfiguration
            {
                Issuer = issuer,
                Audience = audience,
                SecurityKey = securityKey,
                Expiration = datetimeExpiration,
                ExpirationTimeSpan = timespanExpiration
            };
        }
    }
}
