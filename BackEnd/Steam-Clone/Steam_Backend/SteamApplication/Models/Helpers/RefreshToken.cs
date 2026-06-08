namespace SteamApplication.Models.Helpers
{
    public class RefreshToken
    {
        public required Guid UserId { get; set; }
        public required TimeSpan ExpirationInDays { get; set; }
    }
}
