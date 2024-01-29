
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

        public string Encrypt(string text)
        {
            var data = Encoding.UTF8.GetBytes(text);
            var cipher = rsa.Encrypt(data, false);
            return Convert.ToBase64String(cipher);
        }

        public Token GenerateToken(string accountNumber)
        {
            var data = Encoding.UTF8.GetBytes(accountNumber);
            var cipher = rsa.Encrypt(data, false);
            var token = new Token
            {
                AccountNumber = accountNumber,
                Sign = Convert.ToBase64String(cipher)
            };
            return token;
        }

        public bool verifyToken(Token token)
        {
            try{
                var data = Convert.FromBase64String(token.Sign);
                var plain = rsa.Decrypt(data, false);
                return Encoding.UTF8.GetString(plain) == token.AccountNumber;
            } catch {
                return false;
            }
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
            account.AccountNumber = EncryptDbEntry(account.AccountNumber);
            account.Password = EncryptDbEntry(account.Password);
            account.Salt = EncryptDbEntry(account.Salt);
            return account;
        }

        public Account DecryptAccount(Account account){
            account.Name = DecryptDbEntry(account.Name);
            account.Email = DecryptDbEntry(account.Email);
            account.AccountNumber = DecryptDbEntry(account.AccountNumber);
            account.Password = DecryptDbEntry(account.Password);
            account.Salt = DecryptDbEntry(account.Salt);
            return account;
        }
    }
}