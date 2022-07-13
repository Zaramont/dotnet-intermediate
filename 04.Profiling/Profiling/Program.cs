using System;
using System.Security.Cryptography;

namespace Profiling
{
    class Program
    {
        static void Main(string[] args)
        {
            string pwd = "OloloOlolo12345";
            byte[] salt = new byte[16];
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(salt);
            }

            string encryptedPassword = GeneratePasswordHashUsingSalt(pwd, salt);
        }
        private static string GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
        {
            var iterate = 10000;
            byte[] hash;
            using (var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate, HashAlgorithmName.SHA256))
            {
                hash = pbkdf2.GetBytes(20);
            }

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            var passwordHash = Convert.ToBase64String(hashBytes);

            return passwordHash;
        }
    }
}
