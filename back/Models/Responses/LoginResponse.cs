
namespace projekt.Models.Responses
{
    public record LoginResponse
    {
        public LoginResponse()
        {
            AccountNumber = "";
            Balance = 0;

        }
        public required string AccountNumber { get; set; }
        public required decimal Balance { get; set; }

    }
}