
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

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

        /*
            test
            KMbrdiERGMLY5KBgn/8wmQ==
            test
        */

        public static string aes(string text, string key, string mode)
        {
            byte[] encrypted;
            string plaintext;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = Encoding.UTF8.GetBytes(iv_);

                if (mode == "enc"){
                    var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(text);
                            }
                            encrypted = msEncrypt.ToArray();
                        }
                    }
                    return Convert.ToBase64String(encrypted);
                } else if (mode == "dec"){
                    var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (MemoryStream msDecrypt = new MemoryStream( Encoding.UTF8.GetBytes(text)))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {

                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                    return plaintext;
                } else {
                    throw new Exception("Invalid mode");
                }
            }
        }

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