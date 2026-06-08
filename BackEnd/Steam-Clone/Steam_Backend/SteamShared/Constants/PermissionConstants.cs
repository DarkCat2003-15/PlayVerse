namespace SteamShared.Constants
{
    public static class PermissionConstants
    {
        public const string USERS_READ = "USERS_READ";
        public const string USERS_MANAGE = "USERS_MANAGE";
        public const string ROLES_READ = "ROLES_READ";
        public const string ROLES_ASSIGN = "ROLES_ASSIGN";
        public const string CATALOG_MANAGE = "CATALOG_MANAGE";
        public const string GAMES_MANAGE = "GAMES_MANAGE";
        public const string WISHLIST_MANAGE = "WISHLIST_MANAGE";
        public const string LIBRARY_PURCHASE = "LIBRARY_PURCHASE";
        public const string REVIEWS_MANAGE = "REVIEWS_MANAGE";
        public const string FRIENDS_MANAGE = "FRIENDS_MANAGE";
        public const string SESSIONS_PLAY = "SESSIONS_PLAY";
        public const string ACHIEVEMENTS_UNLOCK = "ACHIEVEMENTS_UNLOCK";

        public static readonly string[] All =
        [
            USERS_READ,
            USERS_MANAGE,
            ROLES_READ,
            ROLES_ASSIGN,
            CATALOG_MANAGE,
            GAMES_MANAGE,
            WISHLIST_MANAGE,
            LIBRARY_PURCHASE,
            REVIEWS_MANAGE,
            FRIENDS_MANAGE,
            SESSIONS_PLAY,
            ACHIEVEMENTS_UNLOCK
        ];
    }
}
