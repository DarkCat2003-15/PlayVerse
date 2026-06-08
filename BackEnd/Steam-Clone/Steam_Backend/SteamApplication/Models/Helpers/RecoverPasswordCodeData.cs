namespace SteamApplication.Models.Helpers
{
    public class RecoverPasswordCodeData
    {
        public required Guid UserId { get; set; }
        public required string Email { get; set; }
    }
}
