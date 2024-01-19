
namespace projekt.Models.Responses
{
    public record AccountResponse
    {
        public AccountResponse()
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