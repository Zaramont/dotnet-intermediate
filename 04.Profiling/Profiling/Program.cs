using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Profiling
{
    class Program
    {
        static void Main(string[] args)
        {
            string pwd1 = "OloloOlolo12345";
            // Create a byte array to hold the random value.
            byte[] salt1 = new byte[16];
            using (RNGCryptoServiceProvider rngCsp = new
RNGCryptoServiceProvider())
            {
                // Fill the array with a random value.
                rngCsp.GetBytes(salt1);
            }

            //data1 can be a string or contents of a file.
           // string data1 = "Some test data";
            string encryptedPassword = GeneratePasswordHashUsingSalt(pwd1, salt1);
            Console.WriteLine($"Password - {encryptedPassword}");
            //Console.ReadLine();
        }
        private static string GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
        {

            var iterate = 10000;
            var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            var passwordHash = Convert.ToBase64String(hashBytes);

            return passwordHash;

        }
    }
}
