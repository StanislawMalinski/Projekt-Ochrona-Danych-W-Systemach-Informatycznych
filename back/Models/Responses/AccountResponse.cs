using projekt.Models.Dtos;

namespace projekt.Models.Responses
{
    public record AccountResponse
    {
        public AccountResponse()
        {
            AccountNumber = "";
            Balance = 0;
            History = new List<Transfer>();
        }
        public required string AccountNumber { get; set; }
        public required decimal Balance { get; set; }
        public required List<Transfer>  History {get; set; }
    }
}