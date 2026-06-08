namespace SteamShared.Constants
{
    public static class StoreResponseConstants
    {
        public const string DEVELOPER_NOT_EXISTS = "El desarrollador no existe";
        public const string PUBLISHER_NOT_EXISTS = "El publisher no existe";
        public const string GENRE_NOT_EXISTS = "El genero no existe";
        public const string GENRE_IN_USE = "No se puede eliminar el genero porque esta asociado a juegos";
        public const string GAME_NOT_EXISTS = "El juego no existe";
        public const string GAME_ALREADY_OWNED = "El juego ya pertenece a la biblioteca del usuario";
        public const string GAME_NOT_OWNED = "El usuario no posee el juego en su biblioteca";
        public const string ACHIEVEMENT_NOT_EXISTS = "El logro no existe";
        public const string ACHIEVEMENT_ALREADY_UNLOCKED = "El logro ya fue desbloqueado por el usuario";
        public const string OFFER_INVALID_RANGE = "La oferta debe tener una fecha de inicio menor a la fecha de fin";
        public const string REVIEW_NOT_EXISTS = "La reseña no existe";
        public const string REVIEW_ALREADY_EXISTS = "El usuario ya tiene una reseña activa para este juego";
        public const string FRIEND_NOT_EXISTS = "El amigo no existe";
        public const string FRIEND_ALREADY_EXISTS = "La relacion de amistad ya existe";
        public const string FRIEND_SELF_NOT_ALLOWED = "No puedes agregarte a ti mismo como amigo";
        public const string WISHLIST_ALREADY_EXISTS = "El juego ya existe en la wishlist del usuario";
        public const string WISHLIST_NOT_EXISTS = "El juego no existe en la wishlist del usuario";
        public const string SESSION_NOT_EXISTS = "La sesion no existe";
        public const string SESSION_ALREADY_ACTIVE = "El usuario ya tiene una sesion activa";
        public const string SESSION_ALREADY_FINISHED = "La sesion ya habia sido cerrada";
    }
}
