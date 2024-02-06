using projekt.Services;

namespace projekt.Models.Dtos
{
    public class Token
    {
        public Token()
        {
            Sign = "";
        }
        public int UserId { get; set; }
        public int SessionId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public required string Sign { get; set; }
    }
}