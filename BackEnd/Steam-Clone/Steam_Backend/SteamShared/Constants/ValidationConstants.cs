namespace SteamShared.Constants
{
    public static class ValidationConstants
    {
        public const string USER_NOT_FOUND = "Usuario no encontrado";
        public const string INVALID_DATA = "Datos inválidos";
        public const string MaxLength = "El maximo de caracteres es de {0}";
        public const string MinLength = "El minimo de caracteres es de {1}";
        public const string Required = "La propiedad {0} es requerida";
        public const string VALIDATION_MESSAGE = "Una o más validaciones necesitan atención";
        public const string EMAIL_ADDRESS = "La dirección de correo electrónico, no es correcta {0}";
        public static string IsEmpty(string property) => $"El valor de la propiedad '{property}' es vacio. En casos de UUID, no está admitido '00000000-0000-0000-0000-000000000000'";

    }
}
