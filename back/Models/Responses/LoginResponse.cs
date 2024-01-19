
namespace projekt.Models.Responses
{
    public record LoginResponse
    {
        public LoginResponse()
        {
            AccountNumber = "";
            Balance = 0;
            Currency = "";
        }
        public required string AccountNumber { get; set; }
        public required decimal Balance { get; set; }
        public required string Currency { get; set; }
    }
}