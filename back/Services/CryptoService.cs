
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using projekt.Models.Dtos;
using projekt.Services.Interfaces;

namespace projekt.Services
{
    public class CryptoService : ICryptoService
    {
        private readonly static RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048);
        private readonly IConfiguration _configuration;
        private byte[] _iv;
        private byte[] _key;

        public CryptoService(IConfiguration configuration)
        {
            _configuration = configuration;
            var string_key = _configuration["Secrets:DbSecret"];
            var string_iv =  _configuration["Secrets:InitVector"];
            if (string_iv == null) throw new Exception("Secrets:InitVector not found in appsettings.json");
            if (string_key == null) throw new Exception("Secrets:DbSecret not found in appsettings.json");

            _iv = HashTo128Characters(string_iv);
            _key = HashTo128Characters(string_key);
        }

        private byte[] HashTo128Characters(string input)
        {
            using (SHA512 sha512 = new SHA512Managed())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha512.ComputeHash(inputBytes);
                byte[] truncatedHashBytes = new byte[128 / 8];
                Array.Copy(hashBytes, truncatedHashBytes, truncatedHashBytes.Length);
                return truncatedHashBytes;
            }
        }

        public Token GenerateToken(int userId, int sessionId, DateTime expirationDate)
        {
            var data = Encoding.UTF8.GetBytes(userId.ToString() + sessionId.ToString() + expirationDate.ToString());
            var cipher = rsa.Encrypt(data, false);
            var token = new Token
            {
                UserId = userId,
                SessionId = sessionId,
                ExpirationDate = expirationDate,
                Sign = Convert.ToBase64String(cipher)
            };
            return token;
        }

        public bool VerifyToken(Token token)
        {
            var data = token.UserId.ToString() + token.SessionId.ToString() + token.ExpirationDate.ToString();
            var cipher = Convert.FromBase64String(token.Sign);
            var decrypted = rsa.Decrypt(cipher, false);
            var decryptedString = Encoding.UTF8.GetString(decrypted);
            return decryptedString == data;
        }

        public string HashPassword(string password, string salt)
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

        public string GenerateSalt()
        {
            var salt = new byte[128 / 8];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            return Convert.ToBase64String(salt);
        }

        public string EncryptDbEntry(string phrase){
            byte[] encrypted = EncryptStringToBytes_Aes(phrase, _key, _iv);
            return Convert.ToBase64String(encrypted);
        }

        private byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            byte[] encrypted;
            Aes aesAlg = keyInit(Key, IV);
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            using MemoryStream msEncrypt = new MemoryStream();
            using CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using StreamWriter swEncrypt = new StreamWriter(csEncrypt);      
            swEncrypt.Write(plainText);
            swEncrypt.Close();
            encrypted = msEncrypt.ToArray();
            return encrypted;
        }

        public string DecryptDbEntry(string phrase){
            byte[] encrypted = Convert.FromBase64String(phrase);
            string decrypted = DecryptStringFromBytes_Aes(encrypted, _key, _iv);
            return decrypted;
        }

        private string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            Aes aesAlg = keyInit(Key, IV);
            using var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            using var msDecrypt = new MemoryStream(cipherText);
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);
            string plaintext = srDecrypt.ReadToEnd();

            return plaintext;
        }

        public Aes keyInit(byte[] key, byte[] iv){
            var aes = Aes.Create();
            
            aes.Key = key;
            aes.IV = iv;
            aes.Padding = PaddingMode.PKCS7;
            return aes;
        }

        public Account EncryptAccount(Account account){
            account.Name = EncryptDbEntry(account.Name);
            account.Email = EncryptDbEntry(account.Email);
            account.Password = EncryptDbEntry(account.Password);
            account.Salt = EncryptDbEntry(account.Salt);
            return account;
        }

        public Account DecryptAccount(Account account){
            account.Name = DecryptDbEntry(account.Name);
            account.Email = DecryptDbEntry(account.Email);
            account.Password = DecryptDbEntry(account.Password);
            account.Salt = DecryptDbEntry(account.Salt);
            return account;
        }

        public string EncryptString(string plainText)
        {
            return Convert.ToBase64String(EncryptStringToBytes_Aes(plainText, _key, _iv));
        }

        public string DecryptString(string cipherText)
        {
            return DecryptStringFromBytes_Aes(Convert.FromBase64String(cipherText), _key, _iv);
        }
    }
}