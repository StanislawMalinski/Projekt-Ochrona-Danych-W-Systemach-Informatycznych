using projekt.Services;

namespace projekt.Models.Dtos
{
    public class Token
    {
        public string? AccountNumber { get; set; }
        public string? Sign { get; set; }
        public DateTime Expiration { get; set; }
    }
}