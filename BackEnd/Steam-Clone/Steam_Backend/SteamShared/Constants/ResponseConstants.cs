namespace SteamShared.Constants
{
    public static class ResponseConstants
    {

        // Users
        public const string USER_NOT_EXISTS = "El Usuario no existe";
        public const string USER_EMAIL_TAKED = "Ya existe un usuario con el correo que está argumentando";


        // Projects
        public const string PROJECT_NOT_EXISTS = "El proyecto no existe";

        // Auth
        public const string AUTH_TOKEN_NOT_FOUND = "El token no es correcto, expiró o no se argumentó";
        public const string AUTH_USER_OR_PASSWORD_NOT_FOUND = "Usuario o contraseña incorrectos";
        public const string AUTH_REFRESH_TOKEN_NOT_FOUND = "El token para refrescar la sesión expiró, no existe o es incorrecto";
        public const string AUTH_CLAIM_USER_NOT_FOUND = "No pudo ser validada la identidad del usuario";
        public const string AUTH_REGISTER_TOKEN_NOT_FOUND = "El token de registro no existe o expiró";
        public const string AUTH_REGISTER_EMAIL_MISMATCH = "El correo del formulario no coincide con el correo validado para el registro";
        public const string AUTH_RECOVER_PASSWORD_CODE_NOT_FOUND = "El código de recuperación no existe o expiró";
        public const string AUTH_PASSWORD_ALREADY_USED = "La nueva contraseña no puede ser igual a la actual";
        public const string AUTH_CURRENT_PASSWORD_INVALID = "La contraseña actual es incorrecta";
        public const string ROLE_NOT_EXISTS = "El rol no existe";
        public const string ROLE_ALREADY_ASSIGNED = "El rol ya esta asignado al usuario";

        public static string ErrorUnexpected(string traceId)
        {
            return $"Ha ocurrido un error inesperado: Contacto con soporte, mencionando el siguiente código de error: {traceId}";
        }

        public static string ConfigurationPropertyNotFound(string property)
        {
            return $"Falta la propiedad '{property}' por establecer en la configuración del aplicativo.";
        }
    }
}
