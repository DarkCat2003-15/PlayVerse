using System.Security.Cryptography;
using System.Text;

namespace SteamShared.Helpers
{
    public static class Hasher
    {
        private const char PasswordSeparator = ';';

        public static string HashPassword(string password)
        {
            try
            {
                var salt = RandomSalt();
                var hashData = SHA256.HashData(Encoding.UTF8.GetBytes(password + salt));
                var base64 = Convert.ToBase64String(hashData);

                return $"{base64}{PasswordSeparator}{salt}";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static string HashPassword(string password, string salt)
        {
            try
            {
                var hashData = SHA256.HashData(Encoding.UTF8.GetBytes(password + salt));
                var base64 = Convert.ToBase64String(hashData);

                return $"{base64}{PasswordSeparator}{salt}";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }



        private static string RandomSalt()
        {
            try
            {
                return Convert.ToBase64String(RandomNumberGenerator.GetBytes(128 / 8));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static bool ComparePassword(string password, string hashedPassword)
        {
            try
            {
                if (string.IsNullOrEmpty(hashedPassword))
                    return false;

                var parts = hashedPassword.Split(PasswordSeparator);

                if (parts.Length < 2)
                    return false;

                var salt = parts[1];

                return HashPassword(password, salt) == hashedPassword;
            }
            catch
            {
                return false;
            }
        }

        public static bool ComparePassword(string password1, object password2)
        {
            throw new NotImplementedException();
        }
    }
}
