using projekt.Services;

namespace projekt.Models.Dtos
{
    public class Token
    {
        public string AccountNumber { get; set; }
        public string Sign { get; set; }
        public string Timestamp { get; set; }
        public string Expiration { get; set; }

        public bool isValid()
        {
            return CryptoService.verifyToken(this);
        }
    }
}