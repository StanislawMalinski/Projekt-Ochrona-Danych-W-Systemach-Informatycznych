
using projekt.Models.Dtos;
using  projekt.Services;

namespace projekt.Models.Requests
{
    public class AccountRequest : BasicRequest
    {
        public AccountRequest()
        {
            Email = "";
            token = new Token();
        }
        public required string Email{ get; set; }
        public required Token token { get; set; }

        public override string IsValid()
        {
            if(Validator.validEmail(Email)) return "Email is not valid";
            return "";
        }
    }
}