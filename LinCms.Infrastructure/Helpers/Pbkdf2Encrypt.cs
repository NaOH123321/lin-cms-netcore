using System;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace LinCms.Infrastructure.Helpers
{
    public class Pbkdf2Encrypt
    {
        public static string EncryptPassword(string password)
        {
            var salt = Encoding.Default.GetBytes(password);

            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password,
                salt,
                KeyDerivationPrf.HMACSHA256,
                50000,
                256 / 8));

            var encrypt = $"pbkdf2:sha256:50000${hashed}";

            return encrypt;
        }
    }
}
