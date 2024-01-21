

namespace projekt.Models.Requests
{
    public class AccountRequest
    {
        public AccountRequest()
        {
            AccountNumber = "";
        }
        public required string AccountNumber { get; set; }
    }
}