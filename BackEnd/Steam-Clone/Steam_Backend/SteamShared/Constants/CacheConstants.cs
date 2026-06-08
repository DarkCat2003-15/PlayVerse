namespace SteamShared.Constants
{
    public static class CacheConstants
    {
        public static string AuthToken(string token)
        {
            return $"auth:tokens:{token}";
        }
    }
}
