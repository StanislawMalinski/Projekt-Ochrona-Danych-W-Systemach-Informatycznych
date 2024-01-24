
using projekt.Models.Dtos;
using  projekt.Serivces;
using projekt.Models.Dtos;
namespace projekt.Models.Requests
{
    public class AccountRequest
    {
        public AccountRequest()
        {
            Email = "";
            token = new Token();
        }
        public required string Email{ get; set; }
        public required Token token { get; set; }

        public bool IsValid()
        {
            return Validator.validEmail(Email);
        }
    }
}