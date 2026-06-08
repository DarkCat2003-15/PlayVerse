namespace SteamShared.Constants
{
    public static class UserStatusConstants
    {
        public const int InactiveId = 0;
        public const int ActiveId = 1;

        public const string InactiveCode = "INACTIVE";
        public const string ActiveCode = "ACTIVE";

        public const string InactiveName = "Desactivo";
        public const string ActiveName = "Activo";

        public static string ResolveName(int? statusId)
        {
            return statusId == ActiveId ? ActiveName : InactiveName;
        }

        public static bool IsActive(int? statusId)
        {
            return statusId == ActiveId;
        }
    }
}
