

namespace projekt.Models.Requests
{
    public class AccountRequest
    {
        public AccountRequest()
        {
            Email = "";
        }
        public required string Email{ get; set; }
    }
}