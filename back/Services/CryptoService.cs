
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using projekt.Models.Dtos;

namespace projekt.Serivces
{
    public class CryptoService
    {
        private readonly static RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048);
        private readonly static string iv_ = "0535627058893800";
        public static string Encrypt(string text)
        {
            var data = Encoding.UTF8.GetBytes(text);
            var cipher = rsa.Encrypt(data, false);
            return Convert.ToBase64String(cipher);
        }

        public static Token signToken(string accountNumber)
        {
            var data = Encoding.UTF8.GetBytes(accountNumber);
            var cipher = rsa.Encrypt(data, false);
            var token = new Token();
            token.AccountNumber = accountNumber;
            token.Sign = Convert.ToBase64String(cipher);
            return token;
        }

        public static bool verifyToken(Token token)
        {
            try{
            var data = Convert.FromBase64String(token.Sign);
            var plain = rsa.Decrypt(data, false);
            return Encoding.UTF8.GetString(plain) == token.AccountNumber;
            } catch {
                return false;
            }
        }

        /*
            test
            KMbrdiERGMLY5KBgn/8wmQ==
            test
        */

        public static string GetPublicKey()
        {
            return rsa.ToXmlString(false);
        }

        public static string HashPassword(string password, string salt)
        {
            var bytes = KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.UTF8.GetBytes(salt),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            );
            return Convert.ToBase64String(bytes);
        }

        public static string GenerateSalt()
        {
            var salt = new byte[128 / 8];
            using(var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }
    }
}