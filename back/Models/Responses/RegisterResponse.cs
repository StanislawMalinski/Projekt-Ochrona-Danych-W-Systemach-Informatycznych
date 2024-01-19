

namespace projekt.Models.Responses
{
    public record RegisterResponse
    {
        public RegisterResponse()
        {
            AccountNumber = "";
        }
        public required string AccountNumber { get; set; }
    }
}