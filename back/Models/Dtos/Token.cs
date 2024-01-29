using projekt.Services;

namespace projekt.Models.Dtos
{
    public class Token
    {
        public int UserId { get; set; }
        public int SessionId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string? Sign { get; set; }
    }
}