using System.Security.Cryptography;
using System.Text;

namespace SteamShared.Helpers
{
    public static class Generate
    {
        private const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public static string RandomText(int length = 50)
        {
            var sb = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                sb.Append(Characters[RandomNumberGenerator.GetInt32(Characters.Length)]);
            }

            return sb.ToString();
        }

        public static string NumericCode(int length = 6)
        {
            var sb = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                sb.Append(RandomNumberGenerator.GetInt32(10));
            }

            return sb.ToString();
        }
    }
}
