using System.Security.Cryptography;

namespace Fuwa.Identity
{
    public static class JwtSettings
    {
        public static string Issuer { get; }
        public static string Audience { get; }
        public static string Key { get; }

        static JwtSettings()
        {
            Issuer = "FuwaExampleIssuer";
            Audience = "FuwaExampleAudience";
            Key = GenerateRandomKey();
        }

        private static string GenerateRandomKey()
        {
            var keyBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(keyBytes);
            }
            return Convert.ToBase64String(keyBytes);
        }
    }
}
