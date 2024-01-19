

namespace projekt.Models.Responses
{
    public record PassChangeResponse
    {
        public PassChangeResponse()
        {
            AccountNumber = "";
        }
        public required string AccountNumber { get; set; }
    }
}