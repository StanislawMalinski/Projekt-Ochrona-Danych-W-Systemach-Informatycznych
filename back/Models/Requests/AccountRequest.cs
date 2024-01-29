
using projekt.Models.Dtos;
using  projekt.Services;

namespace projekt.Models.Requests
{
    public class AccountRequest : BasicRequest
    {
        public AccountRequest()
        {
            Token = new Token();
        }
        public required Token Token { get; set; }

        public override string IsValid()
        {
            return "";
        }
    }
}