
using  projekt.Serivces;

namespace projekt.Models.Requests
{
    public class AccountRequest
    {
        public AccountRequest()
        {
            Email = "";
        }
        public required string Email{ get; set; }

        public bool IsValid()
        {
            return Validator.validEmail(Email);
        }
    }
}