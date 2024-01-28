using projekt.Models.Dtos;

namespace projekt.Models.Responses
{
    public class AccountResponse : SimpleResponse
    {
        public AccountResponse()
        {
            AccountNumber = "";
            Balance = 0;
            History = new List<Transfer>();
            Message = "";
            Success = false;
            Token = new Token();
        }
        public AccountResponse(string message)
        {
            AccountNumber = "";
            Balance = 0;
            History = new List<Transfer>();
            Message = message;
            Success = false;
            Token = new Token();
        }
        public required string AccountNumber { get; set; }
        public required decimal Balance { get; set; }
        public required List<Transfer>  History {get; set; }
        public Token Token { get; set; }
    }
}
