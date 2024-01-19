

namespace projekt.Models.Requests
{
    public record AccountRequest
    {
        public AccountRequest()
        {
            AccountNumber = "";
        }
        public required string AccountNumber { get; set; }
    }
}