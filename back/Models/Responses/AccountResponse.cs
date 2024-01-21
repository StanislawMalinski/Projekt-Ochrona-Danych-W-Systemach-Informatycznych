using projekt.Models.Dtos;

namespace projekt.Models.Responses
{
    public class AccountResponse
    {
        public AccountResponse()
        {
            AccountNumber = "";
            Balance = 0;
            History = new List<Transfer>();
            Message = "";
            Success = false;
        }
        public AccountResponse(string message)
        {
            AccountNumber = "";
            Balance = 0;
            History = new List<Transfer>();
            Message = message;
        }
        public required string AccountNumber { get; set; }
        public required decimal Balance { get; set; }
        public required List<Transfer>  History {get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}
