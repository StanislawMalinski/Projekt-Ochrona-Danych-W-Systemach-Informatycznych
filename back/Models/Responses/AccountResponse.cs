using projekt.Models.Dtos;

namespace projekt.Models.Responses
{
    public class AccountResponse : BasicResponse
    {
        public AccountResponse()
        {
            AccountNumber = "";
            Balance = 0;
            History = new List<Transfer>();
            Message = "";
            Success = false;
            Token = new Token(){
                Sign = ""
            };
        }
        public AccountResponse(string message)
        {
            AccountNumber = "";
            Balance = 0;
            History = new List<Transfer>();
            Message = message;
            Success = false;
            Token = new Token(){
                Sign = ""
            };
        }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public List<Transfer>  History {get; set; }
        public Token Token { get; set; }
    }
}
