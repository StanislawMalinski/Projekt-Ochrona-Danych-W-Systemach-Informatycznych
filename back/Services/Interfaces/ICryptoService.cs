
using projekt.Models.Dtos;

namespace projekt.Services.Interfaces
{
    public interface ICryptoService
    {
        public Token GenerateToken(string accountNumber);
        public bool verifyToken(Token token);
        public string HashPassword(string password, string salt);
        public string GenerateSalt();
        public string EncryptString(string plainText);
        public string DecryptString(string cipherText);
        public Account DecryptAccount(Account account);
        public Account EncryptAccount(Account account);
    }
}