using projekt.Serivces;

namespace projekt.Models.Dtos
{
    public class Token
    {
        public string AccountNumber { get; set; }
        public string Sign { get; set; }

        public bool isValid()
        {
            return CryptoService.verifyToken(this);
        }
    }
}